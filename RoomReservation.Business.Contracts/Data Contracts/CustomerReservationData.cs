using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Core.Common.ServiceModel;

namespace RoomReservation.Business.Contracts
{
    [DataContract]
    public class CustomerReservationData : DataContractBase
    {
        [DataMember]
        public int ReservationId { get; set; }

        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public string Room { get; set; }

        [DataMember]
        public DateTime RentalDate { get; set; }

        [DataMember]
        public DateTime ReturnDate { get; set; }

    }
}
