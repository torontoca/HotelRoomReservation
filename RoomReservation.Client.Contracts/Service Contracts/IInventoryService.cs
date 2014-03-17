using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using RoomReservation.Client.Entities;
using Core.Common.Contracts;
using Core.Common.Exceptions;

namespace RoomReservation.Client.Contracts
{
    [ServiceContract]
    public interface IInventoryService : IServiceContract
    {
        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        Room GetRoom(int roomId);

        [OperationContract]
        IEnumerable<Room> GetAllRooms();

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        Room UpdateRoom(Room room);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void DeleteRoom(int roomId);

        [OperationContract]
        IEnumerable<Room> GetAvailableRooms(DateTime pickupDate, DateTime returnDate);

        [OperationContract]
        Task<Room> UpdateRoomAsync(Room room);

        [OperationContract]
        Task DeleteRoomAsync(int roomId);
    }
}
