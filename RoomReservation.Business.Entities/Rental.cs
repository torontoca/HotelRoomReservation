using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Contracts;
using Core.Common.Core;

namespace RoomReservation.Business.Entities
{
    [DataContract]
    public class Rental : EntityBase, IIdentifiableEntity, IAccountOwnedEntity
    {
        [DataMember]
        public int RentalId { get; set; }

        [DataMember]
        public int AccountId { get; set; }

        [DataMember]
        public int RoomId { get; set;}

        [DataMember]
        public DateTime DateRented { get; set; }

        [DataMember]
        public DateTime DateDue { get; set; }

        [DataMember]
        public DateTime? DateReturned { get; set; }

        public int EntityId
        {
            get { return RentalId; }
            set { RentalId = value;}
        }

        public int OwnerAccountId
        {
            get { return AccountId; }
        }

        public override void CopyTo(EntityBase destinationEntity)
        {
            var destinationRental = destinationEntity as Rental;
            if (destinationRental == null) return;

            destinationRental.RentalId = this.RentalId;
            destinationRental.AccountId = this.AccountId;
            destinationRental.ExtensionData = this.ExtensionData;
            destinationRental.EntityId = this.EntityId;
            destinationRental.DateReturned = this.DateReturned;
            destinationRental.DateRented = this.DateRented;
            destinationRental.DateDue = this.DateDue;
            destinationRental.RoomId = this.RoomId;
        }
    }


}
