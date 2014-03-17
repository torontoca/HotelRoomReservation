using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomReservation.Business.Entities;
using Core.Common.Contracts;

namespace RoomReservation.Business.Common
{
    public interface IRoomRentalEngine : IBusinessEngine
    {
        bool IsRoomCurrentlyRented(int roomId, int accountId);

        bool IsRoomCurrentlyRented(int roomId);

        bool IsRoomAvailableForRental(int roomId, DateTime pickupDate, DateTime returnDate,
            IEnumerable<Rental> rentals, IEnumerable<Reservation> reservations);

        Rental RentRoomToCustomer(string loginEmail, int roomId, DateTime rentalDate, DateTime dateDue);
    }
}
