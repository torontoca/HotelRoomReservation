using System.Collections.Generic;
using System.Data.Entity;
using Core.Common.Contracts;

namespace Core.Common.Data
{
    public abstract class DataRepositoryBase<T, U> : IDataRepository<T>
        where T : class, IIdentifiableEntity,new()
        where U : DbContext, new()
    {
        protected abstract T AddEntity(U entityContext, T entity);

        protected abstract T UpdateEntity(U entityContext, T entity);

        protected abstract IEnumerable<T> GetEntities(U entityContext);

        protected abstract T GetEntity(U entityContext, int id);

        //protected abstract void CopyTo(T destinationEntity);

        public T Add(T entity)
        {
            using (var entityContext = new U())
            {
                var entityToAdd = AddEntity(entityContext, entity);

                entityContext.SaveChanges();

                return entityToAdd;
            }
        }

        public void Remove(T entity)
        {
            using (var entityContext = new U())
            {
                entityContext.Entry(entity).State = EntityState.Deleted;
                entityContext.SaveChanges();
            }

        }

        public void Remove(int id)
        {
            using (var entityContext = new U())
            {
                var entityToRemove = Get(id);
                entityContext.Entry(entityToRemove).State = EntityState.Deleted;
                entityContext.SaveChanges();
            }
        }

        public T Update(T entity)
        {
            using (var entityContext = new U())
            {
                var existingEntity = UpdateEntity(entityContext, entity);
                entityContext.SaveChanges();
                return existingEntity;
            }
        }

        public IEnumerable<T> Get()
        {
            using (var entityContext = new U())
            {
                return GetEntities(entityContext);
            }
        }

        public T Get(int id)
        {
            using (var entityContext = new U())
            {
                return GetEntity(entityContext, id);
            }
        }
    }


}
