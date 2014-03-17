using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using RoomReservation.Business.Entities;
using RoomReservation.Data.Contracts.Repository_Interfaces;

namespace RoomReservation.Data.Data_Repositories
{
    [Export(typeof(IAccountRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AccountRepository : DataRepositoryBase<Account>,IAccountRepository
    {
        protected override Account AddEntity(RoomRentalContext entityContext, Account entity)
        {
            return entityContext.AccountSet.Add(entity);
        }

        protected override Account UpdateEntity(RoomRentalContext entityContext, Account entity)
        {
            var existingEntity = entityContext.AccountSet.FirstOrDefault(e => e.AccountId == entity.AccountId);
            
            entity.CopyTo(existingEntity);

            return existingEntity;

            //return (from e in entityContext.AccountSet
            //    where e.AccountId == entity.AccountId
            //    select e).FirstOrDefault();
        }

        protected override IEnumerable<Account> GetEntities(RoomRentalContext entityContext)
        {
            return entityContext.AccountSet.ToList();
            //return (from e in entityContext.AccountSet
            //    select e);
        }

        protected override Account GetEntity(RoomRentalContext entityContext, int id)
        {
            return entityContext.AccountSet.FirstOrDefault(e => e.AccountId == id);
            //return (from e in entityContext.AccountSet
            //        where e.AccountId == id
            //        select e).FirstOrDefault();
        }

        public Account GetByLogin(string login)
        {
            using (var entityContext = new RoomRentalContext())
            {
                return entityContext.AccountSet.FirstOrDefault(e => e.LoginEmail == login);
                //return (from e in entityContext.AccountSet
                //        where e.LoginEmail == login
                //        select e).FirstOrDefault();
            }
        }
    }
}
