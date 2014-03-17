using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using RoomReservation.Client.Entities;
using Core.Common.Contracts;
using Core.Common.Exceptions;

namespace RoomReservation.Client.Contracts
{
    [ServiceContract]
    public interface IRentalService : IServiceContract
    {
        [OperationContract]
        [FaultContract(typeof(AuthorizationValidationException))]
        [FaultContract(typeof(NotFoundException))]
        IEnumerable<Rental> GetRentalHistory(string loginEmail);

        [OperationContract]
        [FaultContract(typeof (AuthorizationValidationException))]
        [FaultContract(typeof (NotFoundException))]
        bool IsRoomCurrentlyRented(int roomId);

        [OperationContract]
        [FaultContract(typeof (AuthorizationValidationException))]
        [FaultContract(typeof (NotFoundException))]
        IEnumerable<Reservation> GetDeadReservations();

        [OperationContract]
        [FaultContract(typeof (AuthorizationValidationException))]
        [FaultContract(typeof (NotFoundException))]
        IEnumerable<CustomerRentalData> GetCurrentRentals();

        [OperationContract]
        [FaultContract(typeof (AuthorizationValidationException))]
        [FaultContract(typeof (NotFoundException))]
        Rental GetRental(int rentalId);

        [OperationContract]
        [FaultContract(typeof (AuthorizationValidationException))]
        [FaultContract(typeof (NotFoundException))]
        IEnumerable<CustomerReservationData> GetCustomerReservations(string loginEmail);

        [OperationContract]
        [FaultContract(typeof (AuthorizationValidationException))]
        [FaultContract(typeof (NotFoundException))]
        IEnumerable<CustomerReservationData> GetCurrentReservations();

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(AuthorizationValidationException))]
        [FaultContract(typeof(NotFoundException))]
        void CancelReservation(int reservationId);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof (AuthorizationValidationException))]
        [FaultContract(typeof (NotFoundException))]
        [FaultContract(typeof(RoomCurrentlyRentedException))]
        [FaultContract(typeof(UnableToRentForDateException))]
        void ExecuteRentalFromReservation(int reservationId);


        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(AuthorizationValidationException))]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(RoomCurrentlyRentedException))]
        [FaultContract(typeof(UnableToRentForDateException))]
        Rental RentRoomToCustomer(string loginEmail, int roomId, DateTime rentalDate, DateTime dateDue);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(AuthorizationValidationException))]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(RoomCurrentlyRentedException))]
        [FaultContract(typeof(UnableToRentForDateException))]
        Reservation MakeReservation(string loginEmail, int roomId, DateTime rentalDate, DateTime dateDue);

        [OperationContract]
        [FaultContract(typeof(AuthorizationValidationException))]
        [FaultContract(typeof(NotFoundException))]
        Reservation GetReservation(int reservationId);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof (RoomNotRentedException))]
        void AccpetRoomReturn(int roomId);






        [OperationContract]
        Task<IEnumerable<Rental>> GetRentalHistoryAsync(string loginEmail);

        [OperationContract]
        Task<bool> IsRoomCurrentlyRentedAsync(int roomId);

        [OperationContract]
        Task<IEnumerable<Reservation>> GetDeadReservationsAsync();

        [OperationContract]
        Task<IEnumerable<CustomerRentalData>> GetCurrentRentalsAsync();

        [OperationContract]
        Task<Rental> GetRentalAsync(int rentalId);

        [OperationContract]
        Task<IEnumerable<CustomerReservationData>> GetCustomerReservationsAsync(string loginEmail);

        [OperationContract]
        Task<IEnumerable<CustomerReservationData>> GetCurrentReservationsAsync();

        [OperationContract]
        Task CancelReservationAsync(int reservationId);

        [OperationContract]
        Task ExecuteRentalFromReservationAsync(int reservationId);


        [OperationContract]
        Task<Rental> RentRoomToCustomerAsync(string loginEmail, int roomId, DateTime rentalDate, DateTime dateDue);

        [OperationContract]
        Task<Reservation> MakeReservationAsync(string loginEmail, int roomId, DateTime rentalDate, DateTime dateDue);

        [OperationContract]
        Task<Reservation> GetReservationAsync(int reservationId);

        [OperationContract]
        Task AccpetRoomReturnAsync(int roomId);


    }
}
