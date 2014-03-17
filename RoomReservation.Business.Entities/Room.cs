using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Contracts;
using Core.Common.Core;

namespace RoomReservation.Business.Entities
{
    [DataContract]
    public class Room : EntityBase, IIdentifiableEntity
    {
        [DataMember]
        public int RoomId { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Color { get; set; }

        [DataMember]
        public int Year { get; set; }

        [DataMember]
        public decimal RentalPrice { get; set; }

        [DataMember]
        public bool CurrentlyRented { get; set; }

        public int EntityId
        {
            get
            {
                return RoomId;
            }

            set
            {
                RoomId = value;
            }
        }

        public override void CopyTo(EntityBase destinationEntity)
        {
            var destinationRoom = destinationEntity as Room;
            if (destinationRoom == null) return;

            destinationRoom.RoomId = this.RoomId;
            destinationRoom.Color = this.Color;
            destinationRoom.CurrentlyRented = this.CurrentlyRented;
            destinationRoom.Description = this.Description;
            destinationRoom.EntityId = this.EntityId;
            destinationRoom.ExtensionData = this.ExtensionData;
            destinationRoom.RentalPrice = this.RentalPrice;
            destinationRoom.Year = this.Year;
            
        }
    }
}
