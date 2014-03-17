using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomReservation.Business.Entities;

namespace RoomReservation.Data.Contracts.DTOs
{
    public class CustomerRentalInfo
    {
        public Account Customer { get; set; }
        public Room Room { get; set; }
        public Rental Rental { get; set; }
    }
}
