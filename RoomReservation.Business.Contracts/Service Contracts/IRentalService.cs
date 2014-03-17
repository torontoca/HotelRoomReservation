using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using RoomReservation.Business.Entities;
using Core.Common.Exceptions;

namespace RoomReservation.Business.Contracts
{
    [ServiceContract]
    public interface IRentalService
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


    }
}
