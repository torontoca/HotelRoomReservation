using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Contracts;
using Core.Common.Core;

namespace RoomReservation.Business.Entities
{
    [DataContract]
    public class Reservation : EntityBase, IIdentifiableEntity,IAccountOwnedEntity
    {
        [DataMember]
        public int ReservationId { get; set; }

        [DataMember]
        public int AccountId { get; set; }

        [DataMember]
        public int RoomId { get; set; }

        [DataMember]
        public DateTime RentalDate { get; set; }

        [DataMember]
        public DateTime ReturnDate { get; set; }

        public int EntityId
        {
            get
            {
                return ReservationId;
            }
            set
            {
                ReservationId = value;
            }
        }

        public int OwnerAccountId
        {
            get { return AccountId; }
        }

        public override void CopyTo(EntityBase destinationEntity)
        {
            var destinationReservation = destinationEntity as Reservation;
            if (destinationReservation == null) return;

            destinationReservation.ReservationId = this.ReservationId;
            destinationReservation.ReturnDate = this.ReturnDate;
            destinationReservation.RentalDate = this.RentalDate;
            destinationReservation.ExtensionData = this.ExtensionData;
            destinationReservation.EntityId = this.EntityId;
            destinationReservation.RoomId = this.RoomId;
            destinationReservation.AccountId = this.AccountId;
        }
    }
}
