using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using RoomReservation.Web.Core;

namespace RoomReservation.Web.Controllers.MVC
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Authorize]
    public class CustomerController : ViewControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [GET("customer/account")]
        public ActionResult MyAccount()
        {
            return View();
        }

        [HttpGet]
        [GET("customer/reserve")]
        public ActionResult ReserveRoom()
        {
            return View();
        }

        [HttpGet]
        [GET("customer/reservations")]
        public ActionResult CurrentReservations()
        {
            return View();
        }

        [HttpGet]
        [GET("customer/history")]
        public ActionResult RentalHistory()
        {
            return View();
        }
    }
}
