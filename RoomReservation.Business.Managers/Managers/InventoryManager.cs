using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Security.Permissions;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using RoomReservation.Business.Common;
using RoomReservation.Business.Contracts;
using RoomReservation.Business.Entities;
using RoomReservation.Common;
using Core.Common.Contracts;
using Core.Common.Core;
using RoomReservation.Data.Contracts.Repository_Interfaces;
using Core.Common.Exceptions;

namespace RoomReservation.Business.Managers
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
        ConcurrencyMode = ConcurrencyMode.Multiple,
        ReleaseServiceInstanceOnTransactionComplete = false)]
    public class InventoryManager : ManagerBase,IInventoryService
    {
        //[Import]
        //protected IDataRepositoryFactory _dataRepositoryFactory;

        [Import]
        protected IBusinessEngineFactory _businessEngineFactory;

        public InventoryManager()
        {
            //ObjectBase.Container.SatisfyImportsOnce(this);
        }

        public InventoryManager(IDataRepositoryFactory dataRepositoryFactory,IBusinessEngineFactory businessEngineFactory)
            :base (dataRepositoryFactory)
        {
           // _dataRepositoryFactory = dataRepositoryFactory;

            _businessEngineFactory = businessEngineFactory;
        }

        [PrincipalPermission(SecurityAction.Demand,Role = Security.RoomRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.RoomRentalUser)]
        public Room GetRoom(int roomId)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var roomRepository = _dataRepositoryFactory.GetDataRepository<IRoomRepository>();
                
                Room roomEntity = roomRepository.Get(roomId);
                if (roomEntity == null)
                {
                    NotFoundException ex = new NotFoundException(string.Format("Room with ID of {0} was not found",roomId));
                    throw new FaultException<NotFoundException>(ex,ex.Message);
                }

                return roomEntity;
            });
            
        }


        [PrincipalPermission(SecurityAction.Demand, Role = Security.RoomRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.RoomRentalUser)]
        public IEnumerable<Room> GetAllRooms()
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var roomRepository = _dataRepositoryFactory.GetDataRepository<IRoomRepository>();
                var allRooms = roomRepository.Get();

                var rentalRepository = _dataRepositoryFactory.GetDataRepository<IRentalRepository>();
                var rentedRooms = rentalRepository.GetCurrentlyRentedRooms();

                foreach (var room in allRooms)
                {
                    var rentedRoom = rentedRooms.FirstOrDefault(e => e.RoomId == room.RoomId);
                    room.CurrentlyRented = (rentedRoom != null);
                }

                return allRooms;
            });
      
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.RoomRentalAdminRole)]
        //[PrincipalPermission(SecurityAction.Demand, Name = Security.RoomRentalUser)]
        public Room UpdateRoom(Room room)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                if (room == null) return null;

                Room roomToUpdate = null;

                var roomRepository = _dataRepositoryFactory.GetDataRepository<IRoomRepository>();

                roomToUpdate = room.RoomId == 0 ? roomRepository.Add(room) : roomRepository.Update(room);

                return roomToUpdate;
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.RoomRentalAdminRole)]
        public void DeleteRoom(int roomId)
        {
            ExecuteFaultHandledOperation(() =>
            {
                var roomRepository = _dataRepositoryFactory.GetDataRepository<IRoomRepository>();

                roomRepository.Remove(roomId);
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.RoomRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.RoomRentalUser)]
        public IEnumerable<Room> GetAvailableRooms(DateTime pickupDate, DateTime returnDate)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var roomRentalEngine = _businessEngineFactory.GetBusinessEngine<IRoomRentalEngine>();

                var roomRepository = _dataRepositoryFactory.GetDataRepository<IRoomRepository>();
                var rentalRepository = _dataRepositoryFactory.GetDataRepository<IRentalRepository>();
                var reservationRepository = _dataRepositoryFactory.GetDataRepository<IReservationRepository>();

                var allRooms = roomRepository.Get();
                var rentedRooms = rentalRepository.GetCurrentlyRentedRooms();
                var reservedRooms = reservationRepository.Get();

                return allRooms.Where(room => roomRentalEngine.IsRoomAvailableForRental(room.RoomId, pickupDate, returnDate, rentedRooms, reservedRooms)).ToList();

            });
        }
    }
}
