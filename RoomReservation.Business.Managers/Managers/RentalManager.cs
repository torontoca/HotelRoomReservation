using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using RoomReservation.Business.Common;
using RoomReservation.Business.Contracts;
using RoomReservation.Business.Entities;
using RoomReservation.Common;
using RoomReservation.Data.Contracts.DTOs;
using RoomReservation.Data.Contracts.Repository_Interfaces;
using Core.Common.Contracts;
using Core.Common.Exceptions;
using VARFLAGS = System.Runtime.InteropServices.ComTypes.VARFLAGS;

namespace RoomReservation.Business.Managers
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
        ConcurrencyMode = ConcurrencyMode.Multiple,
        ReleaseServiceInstanceOnTransactionComplete = false)]
    public class RentalManager : ManagerBase, IRentalService
    {
        //[Import] 
        //private IDataRepositoryFactory _dataRepositoryFactory;

        [Import]
        private IBusinessEngineFactory _businessEngineFactory;
        
        public RentalManager()
        {
        }

        public RentalManager(IDataRepositoryFactory dataRepositoryFactory, IBusinessEngineFactory businessEngineFactory)
            :base(dataRepositoryFactory)
        {
            //_dataRepositoryFactory = dataRepositoryFactory;

            _businessEngineFactory = businessEngineFactory;
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.RoomRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.RoomRentalUser)]
        public IEnumerable<Rental> GetRentalHistory(string loginEmail)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var accountRepository = _dataRepositoryFactory.GetDataRepository<IAccountRepository>();

                var account = accountRepository.GetByLogin(loginEmail);
                if (account == null)
                {
                    NotFoundException ex = new NotFoundException(string.Format("No account was found for login {0}.", loginEmail));
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                ValidateAuthorization(account);

                var rentalRepository = _dataRepositoryFactory.GetDataRepository<IRentalRepository>();

                return rentalRepository.GetRentalHistoryByAccount(account.AccountId);

            });
            
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.RoomRentalAdminRole)]
        public bool IsRoomCurrentlyRented(int roomId)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var roomRentalEngine = _businessEngineFactory.GetBusinessEngine<IRoomRentalEngine>();
                return roomRentalEngine.IsRoomCurrentlyRented(roomId);
            });
        }

        [PrincipalPermission(SecurityAction.Demand,Role = Security.RoomRentalAdminRole)]
        public IEnumerable<Reservation> GetDeadReservations()
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var reservationRepository = _dataRepositoryFactory.GetDataRepository<IReservationRepository>();

                var deadReservations = reservationRepository.GetReservationsByPickupDate(DateTime.Now.AddDays(-1));

                return deadReservations;
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.RoomRentalAdminRole)]
        public IEnumerable<CustomerRentalData> GetCurrentRentals()
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var rentalRepository = _dataRepositoryFactory.GetDataRepository<IRentalRepository>();
                var rentalInfoSet  = rentalRepository.GetCurrentCustomerRentalInfo();

                return rentalInfoSet.Select(rentalInfo => new CustomerRentalData()
                {
                    RentalId = rentalInfo.Rental.RentalId, 
                    Room = rentalInfo.Room.Color + " " + rentalInfo.Room.Year + " " + rentalInfo.Room.Description, 
                    CustomerName = rentalInfo.Customer.FirstName + " " + rentalInfo.Customer.LastName, 
                    DateRented = rentalInfo.Rental.DateRented, 
                    ExpectedReturn = rentalInfo.Rental.DateDue
                }).ToList();

            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.RoomRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.RoomRentalUser)]
        public Rental GetRental(int rentalId)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                Rental result = null;

                var rentalRepository = _dataRepositoryFactory.GetDataRepository<IRentalRepository>();
                result = rentalRepository.Get(rentalId);
                if (result == null)
                {
                    NotFoundException ex = new NotFoundException(string.Format("No rental record was found for id {0}.",rentalId));
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                ValidateAuthorization(result);

                return result;
            });
            
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.RoomRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.RoomRentalUser)]
        public IEnumerable<CustomerReservationData> GetCustomerReservations(string loginEmail)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var accountRepository = _dataRepositoryFactory.GetDataRepository<IAccountRepository>();
                Account account = accountRepository.GetByLogin(loginEmail);
                if (account == null)
                {
                    NotFoundException ex = new NotFoundException(string.Format("No account was found for login {0}.",loginEmail));
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                ValidateAuthorization(account);

                var reservationRepository = _dataRepositoryFactory.GetDataRepository<IReservationRepository>();
                var result = reservationRepository.GetCustomerOpenReservationInfo(account.AccountId)
                        .Select(e => new CustomerReservationData()
                        {
                            ReservationId = e.Reservation.ReservationId,
                            Room = e.Room.Color + " " + e.Room.Year + " " + e.Room.Description,
                            CustomerName = e.Customer.FirstName + " " + e.Customer.LastName,
                            RentalDate = e.Reservation.RentalDate,
                            ReturnDate = e.Reservation.ReturnDate
                        }).ToList();

                return result;
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.RoomRentalAdminRole)]
        public IEnumerable<CustomerReservationData> GetCurrentReservations()
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var reservationRepository = _dataRepositoryFactory.GetDataRepository<IReservationRepository>();
                
                return reservationRepository.GetCurrentCustomerReservationInfo()
                     .Select(e => new CustomerReservationData()
                        {
                            ReservationId = e.Reservation.ReservationId,
                            Room = e.Room.Color + " " + e.Room.Year + " " + e.Room.Description,
                            CustomerName = e.Customer.FirstName + " " + e.Customer.LastName,
                            RentalDate = e.Reservation.RentalDate,
                            ReturnDate = e.Reservation.ReturnDate
                        }).ToList();
            });
        }
        
        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.RoomRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.RoomRentalUser)]
        public void CancelReservation(int reservationId)
        {
            ExecuteFaultHandledOperation(() =>
            {
                var reservationRepository = _dataRepositoryFactory.GetDataRepository<IReservationRepository>();
                Reservation reservation = reservationRepository.Get(reservationId);
                if (reservation == null)
                {
                    NotFoundException ex = new NotFoundException(String.Format("NO reservation was found for Id {0}.",reservationId));
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                ValidateAuthorization(reservation);

                reservationRepository.Remove(reservation);
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.RoomRentalAdminRole)]
        public void ExecuteRentalFromReservation(int reservationId)
        {
            ExecuteFaultHandledOperation(() =>
            {
               var reservationRepository = _dataRepositoryFactory.GetDataRepository<IReservationRepository>();
               Reservation reservation = reservationRepository.Get(reservationId);
               if (reservation == null)
               {
                   NotFoundException ex = new NotFoundException(String.Format("Reservation {0} was not found.", reservationId));
                   throw new FaultException<NotFoundException>(ex, ex.Message);
               }

               var accountRepository = _dataRepositoryFactory.GetDataRepository<IAccountRepository>();
               Account account = accountRepository.Get(reservation.AccountId);
               if (account == null)
               {
                   NotFoundException ex = new NotFoundException(String.Format("No account exists for the reservation {0}.", reservationId));
                   throw new FaultException<NotFoundException>(ex, ex.Message);
               }
                    
               try
                {
                    var roomRentalEngine = _businessEngineFactory.GetBusinessEngine<IRoomRentalEngine>();
                    Rental rental = roomRentalEngine.RentRoomToCustomer(account.LoginEmail, reservation.RoomId, reservation.RentalDate, reservation.ReturnDate);
                }             
                catch (UnableToRentForDateException ex)
                {
                    throw new FaultException<UnableToRentForDateException>(ex,ex.Message);
                }
                catch (RoomCurrentlyRentedException ex)
                {
                   throw new FaultException<RoomCurrentlyRentedException>(ex, ex.Message);
                }
                catch (NotFoundException ex)
                {
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.RoomRentalAdminRole)]
        public Rental RentRoomToCustomer(string loginEmail, int roomId, DateTime rentalDate, DateTime dateDue)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                try
                {
                    var roomRentalEngine = _businessEngineFactory.GetBusinessEngine<IRoomRentalEngine>();
                    return roomRentalEngine.RentRoomToCustomer(loginEmail, roomId, rentalDate, dateDue);
                }
                catch (UnableToRentForDateException ex)
                {
                    throw new FaultException<UnableToRentForDateException>(ex, ex.Message);
                }
                catch (RoomCurrentlyRentedException ex)
                {
                    throw new FaultException<RoomCurrentlyRentedException>(ex, ex.Message);
                }
                catch (NotFoundException ex)
                {
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

            });
            
        }


        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.RoomRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.RoomRentalUser)]
        public Reservation MakeReservation(string loginEmail, int roomId, DateTime rentalDate, DateTime dateDue)
        {
            return ExecuteFaultHandledOperation(() =>
            {

                var accountRepository = _dataRepositoryFactory.GetDataRepository<IAccountRepository>();
                Account account = accountRepository.GetByLogin(loginEmail);
                if (account == null)
                {
                    NotFoundException ex = new NotFoundException(string.Format("No account exists for login {0}.", loginEmail));
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                ValidateAuthorization(account);

                Reservation reservation = new Reservation()
                {
                    AccountId = account.AccountId,
                    RoomId = roomId,
                    RentalDate = rentalDate,
                    ReturnDate = dateDue
                };

                var reservationRepository = _dataRepositoryFactory.GetDataRepository<IReservationRepository>();
                return reservationRepository.Add(reservation);

            });
        }



        [PrincipalPermission(SecurityAction.Demand, Role = Security.RoomRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.RoomRentalUser)]
        public Reservation GetReservation(int reservationId)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                Reservation reservation = null;

                var reservationRepository = _dataRepositoryFactory.GetDataRepository<IReservationRepository>();
                reservation = reservationRepository.Get(reservationId);
                if (reservation == null)
                {
                    NotFoundException ex = new NotFoundException(string.Format("Reservation {0} was not found.",reservationId));
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                ValidateAuthorization(reservation);

                return reservation;
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.RoomRentalAdminRole)]
        public void AccpetRoomReturn(int roomId)
        {
            ExecuteFaultHandledOperation(() =>
            {
                var rentalRepository = _dataRepositoryFactory.GetDataRepository<IRentalRepository>();
                Rental rental = rentalRepository.GetCurrentRentalByRoom(roomId);
                if (rental == null)
                {
                    RoomNotRentedException ex = new RoomNotRentedException(String.Format("Room {0} is not currently rented.",roomId));
                    throw new FaultException<RoomNotRentedException>(ex, ex.Message);
                }

                rental.DateReturned = DateTime.Now;

                rentalRepository.Update(rental);
            });
        }
    }
}
