using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using RoomReservation.Web.Core;
using RoomReservation.Web.Models;


namespace RoomReservation.Web.Controllers.MVC
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AccountController: ViewControllerBase
    {
        private readonly ISecurityAdapter _securityAdapter;

        [ImportingConstructor]
        public AccountController(ISecurityAdapter securityAdapter)
        {
            _securityAdapter = securityAdapter;
        }

        [HttpGet]
        [GET("account/login")]
        public ActionResult Login(string returnUrl)
        {
            _securityAdapter.Initialize();

            return View(new AccountLoginModel(){ ReturnUrl = returnUrl});
        }

        [HttpGet]
        [GET("account/logout")]
        public ActionResult Logout()
        {
            _securityAdapter.Logout();

            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        [GET("account/register")]
        public ActionResult Register()
        {
            _securityAdapter.Initialize();

            return View();
        }


        [HttpGet]
        [GET("account/forgotpassword")]
        [Authorize]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpGet]
        [GET("account/changepassword")]
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }
    }
}