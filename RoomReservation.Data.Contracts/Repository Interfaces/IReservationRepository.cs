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
    public interface IReservationRepository: IDataRepository<Reservation>
    {
        IEnumerable<Reservation> GetReservationsByPickupDate(DateTime pickupDate);
        IEnumerable<CustomerReservationInfo> GetCurrentCustomerReservationInfo();
        IEnumerable<CustomerReservationInfo> GetCustomerOpenReservationInfo(int accountId);
    }
}
