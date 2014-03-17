using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using RoomReservation.Business.Common;
using RoomReservation.Business.Entities;
using RoomReservation.Data.Contracts.Repository_Interfaces;
using Core.Common.Contracts;
using Core.Common.Exceptions;

namespace RoomReservation.Business.Business_Engines
{
    [Export(typeof(IRoomRentalEngine))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class RoomRentalEngine: IRoomRentalEngine
    {

        private IDataRepositoryFactory _dataRepositoryFactory;

        [ImportingConstructor]
        public RoomRentalEngine(IDataRepositoryFactory dataRepositoryFactory)
        {
            _dataRepositoryFactory = dataRepositoryFactory;
        }

        public bool IsRoomAvailableForRental(int roomId, DateTime pickupDate, DateTime returnDate,
            IEnumerable<Rental> rentals, IEnumerable<Reservation> reservations)
        {
            bool isAvailable = true;

            var reservation = reservations.FirstOrDefault(e => e.RoomId == roomId);
            if(reservation != null &&(
                (pickupDate >= reservation.RentalDate && pickupDate <= reservation.ReturnDate) ||
                (returnDate >= reservation.RentalDate && returnDate <= reservation.ReturnDate)))
            {
                isAvailable = false;
            }

            if (isAvailable)
            {
                var rental = rentals.FirstOrDefault(e => e.RoomId == roomId);
                if (rental != null && (pickupDate <= rental.DateDue))
                {
                    isAvailable = false;
                }
            }

            return isAvailable;
        }

        public bool IsRoomCurrentlyRented(int roomId, int accountId)
        {
            bool isRented = false;

            var rentalRepository = _dataRepositoryFactory.GetDataRepository<IRentalRepository>();
            Rental currentRental = rentalRepository.GetCurrentRentalByRoom(roomId);
            if (currentRental != null && currentRental.AccountId == accountId)
            {
                isRented = true;
            }

            return isRented;
        }

        public bool IsRoomCurrentlyRented(int roomId)
        {
            bool isRented = false;

            var rentalRepository = _dataRepositoryFactory.GetDataRepository<IRentalRepository>();
            Rental currentRental = rentalRepository.GetCurrentRentalByRoom(roomId);
            if (currentRental != null)
            {
                isRented = true;
            }

            return isRented;
        }

        public Rental RentRoomToCustomer(string loginEmail, int roomId, DateTime rentalDate, DateTime dateDue)
        {
            if (rentalDate > DateTime.Now)
            {
                throw new UnableToRentForDateException(String.Format("Cannot rent for date {0} yet",rentalDate.ToString()));
            }

            bool roomIsRented = IsRoomCurrentlyRented(roomId);
            if (roomIsRented)
            {
                throw new RoomCurrentlyRentedException(string.Format("Room {0} is already rented",roomId));
            }

            var accountRepository = _dataRepositoryFactory.GetDataRepository<IAccountRepository>();
            Account account = accountRepository.GetByLogin(loginEmail);
            if (account == null)
            {
                throw new NotFoundException(String.Format("No account was found for login {0}.",loginEmail));
            }

            Rental rental = new Rental()
            {
                AccountId = account.AccountId,
                RoomId = roomId,
                DateRented = rentalDate,
                DateDue = dateDue
            };
            var rentalRepository = _dataRepositoryFactory.GetDataRepository<IRentalRepository>();
            Rental savedRental = rentalRepository.Add(rental);

            return savedRental;

        }
    }
}
