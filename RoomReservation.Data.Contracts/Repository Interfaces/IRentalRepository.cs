using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomReservation.Data.Contracts.DTOs;
using Core.Common.Contracts;
using RoomReservation.Business.Entities;

namespace RoomReservation.Data.Contracts.Repository_Interfaces
{
    public interface IRentalRepository : IDataRepository<Rental>
    {
        IEnumerable<Rental> GetRentalHistoryByRoom(int roomId);
        
        Rental GetCurrentRentalByRoom(int roomId);

        IEnumerable<Rental> GetCurrentlyRentedRooms();

        IEnumerable<Rental> GetRentalHistoryByAccount(int accountId);

        IEnumerable<CustomerRentalInfo> GetCurrentCustomerRentalInfo(); 
    }
}
