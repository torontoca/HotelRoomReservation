using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Utils
{
    public static class PropertySupport
    {
        public static string ExtractPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            return propertyExpression == null
                ? null
                : (
                    (propertyExpression.Body as MemberExpression) == null
                        ? null
                        : (propertyExpression.Body as MemberExpression).Member.Name
                    );
        }
    }

   
}
