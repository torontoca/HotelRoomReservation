using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using RoomReservation.Client.Contracts;
using RoomReservation.Client.Entities;
using RoomReservation.Web.Core;
using Core.Common.Contracts;

namespace RoomReservation.Web.Controllers.API
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [System.Web.Http.Authorize]
    [UsesDisposableService]
    public class CustomerController : ApiControllerBase
    {
        [ImportingConstructor]
        public CustomerController(IAccountService accountService)
        {
            _AccountService = accountService;
        }

        IAccountService _AccountService;

        protected override void RegisterServices(List<IServiceContract> disposableServices)
        {
            disposableServices.Add(_AccountService);
        }

        [System.Web.Http.HttpGet]
        [GET("api/customer/getaccount")]
        [System.Web.Http.ActionName("getaccount")]
        public HttpResponseMessage GetCustomerAccountInfo(HttpRequestMessage request)
        {
            return GetHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                Account account = _AccountService.GetCustomerAccountInfo(User.Identity.Name);
                // notice no need to create a seperate model object since Account entity will do just fine

                response = request.CreateResponse<Account>(HttpStatusCode.OK, account);

                return response;
            });
        }

        [System.Web.Http.HttpPost]
        [POST("api/customer/updateaccount")]
        [System.Web.Http.ActionName("updateaccount")]
        public HttpResponseMessage UpdateCustomerAccountInfo(HttpRequestMessage request, Account accountModel)
        {
            return GetHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                ValidateAuthorizedUser(accountModel.LoginEmail);

                List<string> errors = new List<string>();

                //TO DO: Validate the user inputs

                if (errors.Count == 0)
                {
                    _AccountService.UpdateCustomerAccountInfo(accountModel);
                    response = request.CreateResponse(HttpStatusCode.OK);
                }
                else
                    response = request.CreateResponse<string[]>(HttpStatusCode.BadRequest, errors.ToArray());

                return response;
            });
        }
    }
}
