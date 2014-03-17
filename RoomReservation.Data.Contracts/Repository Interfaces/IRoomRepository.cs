using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Contracts;
using RoomReservation.Business.Entities;

namespace RoomReservation.Data.Contracts.Repository_Interfaces
{
    public interface IRoomRepository : IDataRepository<Room>
    {
    }
}
