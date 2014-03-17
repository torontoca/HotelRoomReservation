using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoomReservation.Web.Models
{
    public class AccountRegisterModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public string CreditRoomd { get; set; }

        public string ExpDate { get; set; }

        public string LoginEmail { get; set; }

        public string Password { get; set; }
    }
}