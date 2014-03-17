using System;
using System.Collections.Generic;
using System.Data.Common.EntitySql;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.ServiceModel;
using System.Web;
using System.Web.Http;
using Core.Common.Contracts;
using Core.Common.Exceptions;

namespace RoomReservation.Web.Core
{
    public class ApiControllerBase : ApiController, IServiceAwareController
    {

        protected void ValidateAuthorizedUser(string userRequested)
        {
            var userLoggedIn = User.Identity.Name;
            if (userLoggedIn != userRequested)
            {
                throw new SecurityException("Attempting to access data for another user.");
            }
        }

        protected HttpResponseMessage GetHttpResponse(HttpRequestMessage request,
            Func<HttpResponseMessage> codeToExecute)
        {
            HttpResponseMessage response = null;

            try
            {
                response = codeToExecute.Invoke();
            }
            catch (SecurityException ex)
            {
                response = request.CreateResponse(HttpStatusCode.Unauthorized, ex.Message);
            }
            catch (FaultException<AuthorizationValidationException> ex)
            {
                response = request.CreateResponse(HttpStatusCode.Unauthorized, ex.Message);
            }
            catch (FaultException ex)
            {
                response = request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            catch (Exception ex)
            {
                response = request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return response;

        }


        private List<IServiceContract> _disposableServices;

        protected virtual void RegisterServices(List<IServiceContract> disposableServices)
        {
            
        }

        void IServiceAwareController.RegisterDisposableServices(List<global::Core.Common.Contracts.IServiceContract> disposableServices)
        {
            RegisterServices(disposableServices);
        }

        List<global::Core.Common.Contracts.IServiceContract> IServiceAwareController.DisposableServices
        {
            get
            {
                return _disposableServices ?? (_disposableServices = new List<IServiceContract>());
            }
        }
    }
}