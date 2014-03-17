using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using RoomReservation.Business.Contracts;
using RoomReservation.Business.Entities;
using RoomReservation.Common;
using RoomReservation.Data.Contracts.Repository_Interfaces;
using Core.Common.Contracts;
using Core.Common.Exceptions;

namespace RoomReservation.Business.Managers
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
        ConcurrencyMode = ConcurrencyMode.Multiple,
        ReleaseServiceInstanceOnTransactionComplete = false)]
    public class AccountManager: ManagerBase,IAccountService
    {
        public AccountManager()
        {

        }

        public AccountManager(IDataRepositoryFactory dataRepositoryFactory)
            :base(dataRepositoryFactory)
        {
            
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.RoomRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.RoomRentalUser)]
        public Account GetCustomerAccountInfo(string loginEmail)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                Account result = null;

                var accountRepository = _dataRepositoryFactory.GetDataRepository<IAccountRepository>();
                result = accountRepository.GetByLogin(loginEmail);
                if (result == null)
                {
                    NotFoundException ex = new NotFoundException(String.Format("No account was found for login {0}.",loginEmail));
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                ValidateAuthorization(result);

                return result;

            });
        }


        [PrincipalPermission(SecurityAction.Demand, Role = Security.RoomRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.RoomRentalUser)]
        [OperationBehavior(TransactionScopeRequired = true)]
        public void UpdateCustomerAccountInfo(Account account)
        {
            ExecuteFaultHandledOperation(() =>
            {
                ValidateAuthorization(account);

                var accountRepository = _dataRepositoryFactory.GetDataRepository<IAccountRepository>();
                accountRepository.Update(account);
            });
        }
    }
}
