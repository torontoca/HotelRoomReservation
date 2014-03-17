using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using RoomReservation.Client.Contracts;
using Core.Common.ServiceModel;

namespace RoomReservation.Client.Proxies
{
    [Export(typeof(IInventoryService))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class InventoryClient:UserClientBase<IInventoryService>, IInventoryService
    {
        public RoomReservation.Client.Entities.Room GetRoom(int roomId)
        {
            return ExecuteFaultHandledOperation(() => Channel.GetRoom(roomId));
       
        }

        public IEnumerable<RoomReservation.Client.Entities.Room> GetAllRooms()
        {
            return ExecuteFaultHandledOperation(() => Channel.GetAllRooms());
        }

        public RoomReservation.Client.Entities.Room UpdateRoom(RoomReservation.Client.Entities.Room room)
        {
            return ExecuteFaultHandledOperation(() => Channel.UpdateRoom(room));
        }

        public void DeleteRoom(int roomId)
        {
            ExecuteFaultHandledOperation(() => Channel.DeleteRoom(roomId));
        }

        public IEnumerable<RoomReservation.Client.Entities.Room> GetAvailableRooms(DateTime pickupDate, DateTime returnDate)
        {
            return ExecuteFaultHandledOperation(() => Channel.GetAvailableRooms(pickupDate, returnDate));
             
        }

        public Task<RoomReservation.Client.Entities.Room> UpdateRoomAsync(RoomReservation.Client.Entities.Room room)
        {
            return ExecuteFaultHandledOperation(() => Channel.UpdateRoomAsync(room));
        }

        public Task DeleteRoomAsync(int roomId)
        {
            return ExecuteFaultHandledOperation(() => Channel.DeleteRoomAsync(roomId));
        }

    }
}
