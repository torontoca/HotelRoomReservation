using System.ComponentModel.Composition;
using System.Dynamic;
using RoomReservation.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomReservation.Data.Contracts.Repository_Interfaces;
using RoomReservation.Data.Contracts.DTOs;

namespace RoomReservation.Data.Data_Repositories
{
    [Export(typeof(IRentalRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class RentalRepository : DataRepositoryBase<Rental>,IRentalRepository
    {
        protected override Rental AddEntity(RoomRentalContext entityContext, Rental entity)
        {
            return entityContext.RentalSet.Add(entity);
        }

        protected override Rental UpdateEntity(RoomRentalContext entityContext, Rental entity)
        {
            var existingEntity = entityContext.RentalSet.FirstOrDefault(e => e.RentalId == entity.RentalId);

            entity.CopyTo(existingEntity);

            return existingEntity;
        }

        protected override IEnumerable<Rental> GetEntities(RoomRentalContext entityContext)
        {
            return entityContext.RentalSet.ToList();
        }

        protected override Rental GetEntity(RoomRentalContext entityContext, int id)
        {
            return entityContext.RentalSet.FirstOrDefault(e => e.RentalId == id);
        }

        public IEnumerable<Rental> GetRentalHistoryByRoom(int roomId)
        {
            using (var entityContext = new RoomRentalContext())
            {
                return entityContext.RentalSet.Where(e => e.RoomId == roomId).ToList(); 
            }
        }

        public Rental GetCurrentRentalByRoom(int roomId)
        {
            using (var entityContext = new RoomRentalContext())
            {
                return entityContext.RentalSet.FirstOrDefault(e => e.RoomId == roomId && e.DateReturned == null);
            }
        }

        public IEnumerable<Rental> GetCurrentlyRentedRooms()
        {
            using (var entityContext = new RoomRentalContext())
            {
                return entityContext.RentalSet.Where(e => e.DateReturned == null).ToList();
            }
        }


        public IEnumerable<Rental> GetRentalHistoryByAccount(int accountId)
        {
            using (var entityContext = new RoomRentalContext())
            {
                return entityContext.RentalSet.Where(e => e.AccountId == accountId).ToList();
            }
        }

        public IEnumerable<CustomerRentalInfo> GetCurrentCustomerRentalInfo()
        {
            using (var entityContext = new RoomRentalContext())
            {
                return (from r in entityContext.RentalSet
                    where r.DateReturned == null
                    join a in entityContext.AccountSet on r.AccountId equals a.AccountId
                    join c in entityContext.RoomSet on r.RoomId equals c.RoomId
                    select new CustomerRentalInfo()
                    {
                        Customer = a,
                        Room = c,
                        Rental = r
                    }).ToList();
            }
        }
    }
}
