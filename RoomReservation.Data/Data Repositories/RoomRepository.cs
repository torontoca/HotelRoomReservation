using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomReservation.Business.Entities;
using RoomReservation.Data.Contracts.Repository_Interfaces;

namespace RoomReservation.Data.Data_Repositories
{
    [Export(typeof(IRoomRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class RoomRepository : DataRepositoryBase<Room>, IRoomRepository
    {
        protected override Room AddEntity(RoomRentalContext entityContext, Room entity)
        {
            return entityContext.RoomSet.Add(entity);
        }

        protected override Room UpdateEntity(RoomRentalContext entityContext, Room entity)
        {
            var existingEntity = entityContext.RoomSet.FirstOrDefault(e => e.RoomId == entity.RoomId);

            entity.CopyTo(existingEntity);

            return existingEntity;
        }

        protected override IEnumerable<Room> GetEntities(RoomRentalContext entityContext)
        {
            return entityContext.RoomSet.ToList();
        }

        protected override Room GetEntity(RoomRentalContext entityContext, int id)
        {
            return entityContext.RoomSet.FirstOrDefault(e => e.RoomId == id);
        }
    }
}
