using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RoomReservation.Business.Entities;
using RoomReservation.Common;
using RoomReservation.Data.Contracts.Repository_Interfaces;
using Core.Common.Contracts;
using Core.Common.Core;
using Core.Common.Exceptions;

namespace RoomReservation.Business.Managers
{
    public class ManagerBase
    {
        private string _loginName;

        private Account _authorizationAccount;

        [Import] 
        protected IDataRepositoryFactory _dataRepositoryFactory;

        protected ManagerBase() : this(null)
        {
        }

        protected ManagerBase(IDataRepositoryFactory dataRepositoryFactory)
        {
            if (dataRepositoryFactory != null)
            {
                _dataRepositoryFactory = dataRepositoryFactory;
            }
            else
            {
                if (ObjectBase.Container != null)
                {
                    ObjectBase.Container.SatisfyImportsOnce(this);
                }
            }
                
            var operationContext = OperationContext.Current;
            if (operationContext != null)
            {
                _loginName = operationContext.IncomingMessageHeaders.GetHeader<String>("String", "System");
               // _loginName = "sonwave2008@gmail.com";
                if (_loginName.IndexOf(@"\",StringComparison.InvariantCulture) > 1)
                {
                    _loginName = String.Empty;
                }
            }

            if (!String.IsNullOrEmpty(_loginName))
            {
                _authorizationAccount = LoadAuthorizationAccount(_loginName);
            }

           
            
        }

        protected Account LoadAuthorizationAccount(string loginName)
        {
            var accountRepository = _dataRepositoryFactory.GetDataRepository<IAccountRepository>();

            _authorizationAccount =  accountRepository.GetByLogin(loginName);
            if (_authorizationAccount == null)
            {
                NotFoundException ex = new NotFoundException(string.Format("Cannot find any account for login: {0}.", loginName));
                throw new FaultException<NotFoundException>(ex, ex.Message);
            }

            return _authorizationAccount;
        }

        protected void ValidateAuthorization(IAccountOwnedEntity entity)
        {
            if (!Thread.CurrentPrincipal.IsInRole(Security.RoomRentalAdminRole))
            {
                if (_authorizationAccount != null)
                {
                    if (_loginName != String.Empty && entity.OwnerAccountId != _authorizationAccount.AccountId)
                    {
                        AuthorizationValidationException ex = new AuthorizationValidationException("You are not allowed to access other's account information.");
                        throw new FaultException<AuthorizationValidationException>(ex, ex.Message);
                    }
                }
            }
        }

        protected T ExecuteFaultHandledOperation<T>(Func<T> codeToExecute)
        {
            try
            {
                return codeToExecute.Invoke();
            }
            catch (FaultException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
              
        }


        protected void ExecuteFaultHandledOperation(Action codeToExecute)
        {
            try
            {
                codeToExecute.Invoke();
            }
            catch (FaultException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }

        }
    }
}
