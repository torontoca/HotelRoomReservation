using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoomReservation.Web.Models
{
    public class ReservationModel
    {
        public int Room { get; set; }

        public DateTime PickupDate { get; set; }

        public DateTime ReturnDate { get; set; }
    }
}