using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using RoomReservation.Client.Contracts;
using RoomReservation.Client.Entities;
using Core.Common.ServiceModel;

namespace RoomReservation.Client.Proxies
{
    [Export(typeof(IRentalService))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class RentalClient : UserClientBase<IRentalService>, IRentalService
    {
        public IEnumerable<Entities.Rental> GetRentalHistory(string loginEmail)
        {
            return ExecuteFaultHandledOperation(() => Channel.GetRentalHistory(loginEmail));
        }

        public bool IsRoomCurrentlyRented(int roomId)
        {
            return ExecuteFaultHandledOperation(() => Channel.IsRoomCurrentlyRented(roomId));
        }

        public IEnumerable<Entities.Reservation> GetDeadReservations()
        {
            return ExecuteFaultHandledOperation(() => Channel.GetDeadReservations());
        }

        public IEnumerable<CustomerRentalData> GetCurrentRentals()
        {
            return ExecuteFaultHandledOperation(() => Channel.GetCurrentRentals());
        }

        public Entities.Rental GetRental(int rentalId)
        {
            return ExecuteFaultHandledOperation(() => Channel.GetRental(rentalId));
        }

        public IEnumerable<CustomerReservationData> GetCustomerReservations(string loginEmail)
        {
            return ExecuteFaultHandledOperation(() => Channel.GetCustomerReservations(loginEmail));
        }

        public IEnumerable<CustomerReservationData> GetCurrentReservations()
        {
            return ExecuteFaultHandledOperation(() => Channel.GetCurrentReservations());
        }

        public void CancelReservation(int reservationId)
        {
            ExecuteFaultHandledOperation(() => Channel.CancelReservation(reservationId));
        
        }

        public void ExecuteRentalFromReservation(int reservationId)
        {
            ExecuteFaultHandledOperation(() => Channel.ExecuteRentalFromReservation(reservationId));
            
        }

        public Entities.Rental RentRoomToCustomer(string loginEmail, int roomId, DateTime rentalDate, DateTime dateDue)
        {
            return ExecuteFaultHandledOperation(() => Channel.RentRoomToCustomer(loginEmail, roomId, rentalDate, dateDue));
        }

        public Entities.Reservation MakeReservation(string loginEmail, int roomId, DateTime rentalDate, DateTime dateDue)
        {
            return ExecuteFaultHandledOperation(() => Channel.MakeReservation(loginEmail, roomId, rentalDate, dateDue));
        }

        public Entities.Reservation GetReservation(int reservationId)
        {
            return ExecuteFaultHandledOperation(() => Channel.GetReservation(reservationId));
        }

        public void AccpetRoomReturn(int roomId)
        {
            ExecuteFaultHandledOperation(() => Channel.AccpetRoomReturn(roomId));
        }

        public Task<IEnumerable<Entities.Rental>> GetRentalHistoryAsync(string loginEmail)
        {
            return ExecuteFaultHandledOperation(() => Channel.GetRentalHistoryAsync(loginEmail));
        }

        public Task<bool> IsRoomCurrentlyRentedAsync(int roomId)
        {
            return ExecuteFaultHandledOperation(() => Channel.IsRoomCurrentlyRentedAsync(roomId));
        }

        public Task<IEnumerable<Entities.Reservation>> GetDeadReservationsAsync()
        {
            return ExecuteFaultHandledOperation(() => Channel.GetDeadReservationsAsync());
        }

        public Task<IEnumerable<CustomerRentalData>> GetCurrentRentalsAsync()
        {
            return ExecuteFaultHandledOperation(() => Channel.GetCurrentRentalsAsync());
            
        }

        public Task<Entities.Rental> GetRentalAsync(int rentalId)
        {
            return ExecuteFaultHandledOperation(() => Channel.GetRentalAsync(rentalId));
        }

        public Task<IEnumerable<CustomerReservationData>> GetCustomerReservationsAsync(string loginEmail)
        {
            return ExecuteFaultHandledOperation(() => Channel.GetCustomerReservationsAsync(loginEmail));
        }

        public Task<IEnumerable<CustomerReservationData>> GetCurrentReservationsAsync()
        {
            return ExecuteFaultHandledOperation(() => Channel.GetCurrentReservationsAsync());
           
        }

        public Task CancelReservationAsync(int reservationId)
        {
            return ExecuteFaultHandledOperation(() => Channel.CancelReservationAsync(reservationId));
           
        }

        public Task ExecuteRentalFromReservationAsync(int reservationId)
        {
            return ExecuteFaultHandledOperation(() => Channel.ExecuteRentalFromReservationAsync(reservationId));
        }

        public Task<Entities.Rental> RentRoomToCustomerAsync(string loginEmail, int roomId, DateTime rentalDate, DateTime dateDue)
        {
            return ExecuteFaultHandledOperation(() => Channel.RentRoomToCustomerAsync(loginEmail, roomId, rentalDate, dateDue));
        }

        public Task<Entities.Reservation> MakeReservationAsync(string loginEmail, int roomId, DateTime rentalDate, DateTime dateDue)
        {
            return ExecuteFaultHandledOperation(() => Channel.MakeReservationAsync(loginEmail, roomId, rentalDate, dateDue));
        }

        public Task<Entities.Reservation> GetReservationAsync(int reservationId)
        {
            return ExecuteFaultHandledOperation(() => Channel.GetReservationAsync(reservationId));
        }

        public Task AccpetRoomReturnAsync(int roomId)
        {
            return ExecuteFaultHandledOperation(() => Channel.AccpetRoomReturnAsync(roomId));
        }
    }
}
