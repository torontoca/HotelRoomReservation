using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using RoomReservation.Data.Contracts.Repository_Interfaces;
using RoomReservation.Business.Entities;
using RoomReservation.Data.Contracts.DTOs;

namespace RoomReservation.Data.Data_Repositories
{
    [Export(typeof(IReservationRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ReservationRepository: DataRepositoryBase<Reservation>, IReservationRepository
    {

        protected override Reservation AddEntity(RoomRentalContext entityContext, Reservation entity)
        {
            return entityContext.ReservationSet.Add(entity);
        }

        protected override Reservation UpdateEntity(RoomRentalContext entityContext, Reservation entity)
        {
            var existingEntity = entityContext.ReservationSet.FirstOrDefault(e => e.ReservationId == entity.ReservationId);

            entity.CopyTo(existingEntity);

            return existingEntity;
        }

        protected override IEnumerable<Reservation> GetEntities(RoomRentalContext entityContext)
        {
            return entityContext.ReservationSet.ToList();
        }

        protected override Reservation GetEntity(RoomRentalContext entityContext, int id)
        {
            return entityContext.ReservationSet.FirstOrDefault(e => e.ReservationId == id);
        }



        public IEnumerable<Reservation> GetReservationsByPickupDate(DateTime pickupDate)
        {
            using (var entityContext = new RoomRentalContext())
            {
                return entityContext.ReservationSet.Where(e => e.RentalDate < pickupDate).ToList();
            }
        }


        public IEnumerable<CustomerReservationInfo> GetCurrentCustomerReservationInfo()
        {
            using (var entityContext = new RoomRentalContext())
            {
                return (from r in entityContext.ReservationSet
                    join a in entityContext.AccountSet on r.AccountId equals a.AccountId
                    join c in entityContext.RoomSet on r.RoomId equals c.RoomId
                    select new CustomerReservationInfo()
                    {
                        Customer = a,
                        Room = c,
                        Reservation = r
                    }).ToList();
            }
        }

        public IEnumerable<CustomerReservationInfo> GetCustomerOpenReservationInfo(int accountId)
        {
            using (var entityContext = new RoomRentalContext())
            {
                var result = (from r in entityContext.ReservationSet
                        where r.AccountId == accountId
                        join a in entityContext.AccountSet on r.AccountId equals a.AccountId
                        join c in entityContext.RoomSet on r.RoomId equals c.RoomId
                        select new CustomerReservationInfo()
                        {
                            Customer = a,
                            Room = c,
                            Reservation = r
                        }).ToList();

                return result;
            }
        }
    }
}
