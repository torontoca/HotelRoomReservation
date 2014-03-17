using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition.Hosting;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Configuration;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Utils;
using FluentValidation;
using FluentValidation.Results;

namespace Core.Common.Core
{
    public class ObjectBase : INotifyPropertyChanged,IDataErrorInfo
    {
        public static CompositionContainer Container { get; set; }

        public ObjectBase()
        {
            
        }
               
        protected IValidator _validator;
        protected IEnumerable<ValidationFailure> _validationFailures;
        [NotNavigable]
        public IEnumerable<ValidationFailure> ValidationFailures
        {
            get { return _validationFailures; }
        }

        protected virtual IValidator GetValidator()
        {
            return null;
        }

        public void Validate()
        {
            if (_validator == null) return;

            var result = _validator.Validate(this);
            _validationFailures = result.Errors;
        }

        public virtual bool IsValid
        {
            get { return _validationFailures.HasAnyElement(); }
        }

        private event PropertyChangedEventHandler _PropertyChanged;
        private readonly List<PropertyChangedEventHandler> _propertyChangedEventHandlers = new List<PropertyChangedEventHandler>(); 
        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                if (_propertyChangedEventHandlers.Contains(value)) return;
                _PropertyChanged += value;
                _propertyChangedEventHandlers.Add(value);
            }

            remove
            {
                if (!_propertyChangedEventHandlers.Contains(value)) return;
                _PropertyChanged -= value;
                _propertyChangedEventHandlers.Remove(value);
            }
        }

        public IEnumerable<ObjectBase> GetDirtyObjects()
        {
            var dirtyObjects = new List<ObjectBase>();

            WalkObjectGraph(
                (o) =>
                {
                    if (o.IsDirty)
                    {
                        dirtyObjects.Add(o);
                    }

                    return false;
                },
                null
            );

            return dirtyObjects;
        }

        public void ClearAll()
        {
            WalkObjectGraph(
                o =>
                {
                    if (o.IsDirty)
                    {
                        o.IsDirty = false;
                    }

                    return false;
                },
                null
                );
        }

        public virtual bool IsAnythingDirty()
        {
            var isDirty = false;

            WalkObjectGraph(
                o =>
                {
                    if (o.IsDirty)
                    {
                        isDirty = true;
                    }

                    return isDirty;
                },
                null
                );

            return isDirty;
        }

        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            var propertyName = PropertySupport.ExtractPropertyName(propertyExpression);
            OnPropertyChanged(propertyName,true);
        }

        private bool _isDirty;
        [NotNavigable]
        public bool IsDirty
        {
            get { return _isDirty; }
            set { _isDirty = value; }
        }

        #region Helper Functions
        protected virtual void OnPropertyChanged(string propertyName, bool makeDirty)
        {
            if (_PropertyChanged == null || String.IsNullOrEmpty(propertyName)) return;

            _PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

            if (makeDirty)
            {
                _isDirty = true;
            }
        }

        protected void WalkObjectGraph(Func<ObjectBase, bool> snippetForObject,
                                       Action<IList> snippetForCollection,
                                       params string[] exemptProperties)
        {
            var visitedObjects = new List<ObjectBase>();
            var excludedProperties = exemptProperties == null ? new List<string>() : exemptProperties.ToList();
            Action<ObjectBase> walk = null;

            walk = ((o) =>
            {
                if (visitedObjects.Contains(o) || o == null) return;

                if (snippetForObject == null)
                {
                    throw new ArgumentException("snippetForObject cannot be null.");
                }

                visitedObjects.Add(o);

                var exitWalk = snippetForObject(o);
                if (exitWalk) return;

                var properties = o.GetBrowsableProperties();
                foreach (PropertyInfo property in properties.Where(property => !excludedProperties.Contains(property.Name)))
                {
                    if (property.PropertyType.IsSubclassOf(typeof (ObjectBase)))
                    {
                        walk(property.GetValue(o, null) as ObjectBase);
                    }
                    else
                    {
                        var collection = property.GetValue(o, null) as IList;
                        if (collection == null) continue;

                        if (snippetForCollection != null)
                        {
                            snippetForCollection(collection);
                        }

                        foreach (ObjectBase item in collection.OfType<ObjectBase>())
                        {
                            walk(item as ObjectBase);
                        }
                    }
                }
            });

            walk(this);

        }

        protected IEnumerable<PropertyInfo> GetBrowsableProperties()
        {
            var properties = this.GetType().GetProperties().ToList();
            return properties.Where((propertyInfo) => (!Attribute.IsDefined(propertyInfo, typeof (NotNavigableAttribute))));
        }
        #endregion

        string IDataErrorInfo.Error
        {
            get { return String.Empty; }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (!_validationFailures.HasAnyElement()) return String.Empty;

                var errors = new StringBuilder();
                foreach (ValidationFailure validationFailure in _validationFailures.Where(validationFailure => validationFailure.PropertyName.Equals(columnName)))
                {
                    errors.AppendLine(validationFailure.ErrorMessage);
                }
                return errors.ToString();
            }
        }
    }
}