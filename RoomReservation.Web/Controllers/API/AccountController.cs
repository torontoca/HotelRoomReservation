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
using RoomReservation.Web.Core;
using RoomReservation.Web.Models;

namespace RoomReservation.Web.Controllers.API
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AccountController : ApiControllerBase
    {
        private ISecurityAdapter _securityAdapter;

        [ImportingConstructor]
        public AccountController(ISecurityAdapter securityAdapter)
        {
            _securityAdapter = securityAdapter;
        }

        [System.Web.Http.HttpPost]
        [POST("api/account/login")]
       // [System.Web.Http.ActionName("DefaultAction")]
        public HttpResponseMessage Login( HttpRequestMessage request,[FromBody] AccountLoginModel accountModel)
        {
            return GetHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                var isSuccess = _securityAdapter.Login(accountModel.LoginEmail, accountModel.Password,
                    accountModel.RememberMe);

                response = isSuccess == true
                    ? request.CreateResponse(HttpStatusCode.OK)
                    : request.CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized login attempt.");

                return response; 
            });
            
        }

        [System.Web.Http.HttpPost]
        [POST("api/account/validateregistrationstep1")]
       // [System.Web.Http.ActionName("register/validate1")]
        public HttpResponseMessage ValidateRegistrationStep1(HttpRequestMessage request,
            [FromBody] AccountRegisterModel accountModel)
        {
            return GetHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                var errors = new List<string>();

                //TO DO : check the accuracy of province and zipcode

                response = errors.Count == 0
                    ? request.CreateResponse(HttpStatusCode.OK)
                    : request.CreateResponse<string[]>(HttpStatusCode.BadRequest, errors.ToArray());

                return response;
            });
        }


        [System.Web.Http.HttpPost]
        [POST("api/account/validateregistrationstep2")]
        public HttpResponseMessage ValidateRegistrationStep2(HttpRequestMessage request,
            [FromBody] AccountRegisterModel accountModel)
        {
            return GetHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                response = _securityAdapter.UserExists(accountModel.LoginEmail) == false
                    ? request.CreateResponse(HttpStatusCode.OK)
                    : request.CreateResponse<string[]>(HttpStatusCode.BadRequest, new List<string>(){"An account with this email is already registered."}.ToArray());

                return response;
            });
        }



        [System.Web.Http.HttpPost]
        [POST("api/account/validateregistrationstep1")]
        public HttpResponseMessage ValidateRegistrationStep3(HttpRequestMessage request,
            [FromBody] AccountRegisterModel accountModel)
        {
            return GetHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                var errors = new List<string>();

                //TO DO : check the credit roomd  and expiration date

                response = errors.Count == 0
                    ? request.CreateResponse(HttpStatusCode.OK)
                    : request.CreateResponse<string[]>(HttpStatusCode.BadRequest, errors.ToArray());

                return response;
            });
        }



        [System.Web.Http.HttpPost]
        [POST("api/account/register")]
        [System.Web.Http.ActionName("register")]
        public HttpResponseMessage CreateAccount(HttpRequestMessage request,
            [FromBody] AccountRegisterModel accountModel)
        {
            return GetHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (ValidateRegistrationStep1(request, accountModel).IsSuccessStatusCode &&
                    ValidateRegistrationStep2(request, accountModel).IsSuccessStatusCode &&
                    ValidateRegistrationStep3(request, accountModel).IsSuccessStatusCode)
                {
                    _securityAdapter.Register(accountModel.LoginEmail,accountModel.Password,
                        propertyValues: new
                        {
                            FirstName = accountModel.FirstName,
                            LastName = accountModel.LastName,
                            Address = accountModel.Address,
                            City = accountModel.City,
                            State = accountModel.State,
                            ZipCode = accountModel.ZipCode,
                            CreditRoomd = accountModel.CreditRoomd,
                            ExpDate = accountModel.ExpDate.Substring(0,2) + accountModel.ExpDate.Substring(3,2)
                        });

                    _securityAdapter.Login(accountModel.LoginEmail, accountModel.Password, false);

                    response = request.CreateResponse(HttpStatusCode.OK);
                }
                
                return response;
            });
        }

        [System.Web.Http.HttpPost]
        [POST("api/account/changepw")]
        [System.Web.Http.ActionName("changepw")]
        public HttpResponseMessage ChangePassword(HttpRequestMessage request,
            [FromBody] AccountChangePasswordModel passowordModel)
        {
            return GetHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                ValidateAuthorizedUser(passowordModel.LoginEmail);

                response = _securityAdapter.ChangePassword(passowordModel.LoginEmail, passowordModel.OldPassword,
                    passowordModel.NewPassword)
                    ? request.CreateResponse(HttpStatusCode.OK)
                    : request.CreateResponse(HttpStatusCode.InternalServerError, "Unable to change password.");
                
                return response;
            });
        }

    }
}
