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
    public class Account : EntityBase, IIdentifiableEntity, IAccountOwnedEntity
    {
        [DataMember]
        public int AccountId { get; set; }

        [DataMember]
        public string LoginEmail { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string State { get; set; }

        [DataMember]
        public string ZipCode { get; set; }

        [DataMember]
        public string CreditRoomd { get; set; }

        [DataMember]
        public string ExpDate { get; set; }


        public int EntityId
        {
            get
            {
                return AccountId;
            }
            set
            {
                AccountId = value;
            }
        }

        public override void CopyTo(EntityBase destinationEntity)
        {
            var destinationAccount = destinationEntity as Account;
            if (destinationAccount == null) return;

            destinationAccount.AccountId = this.AccountId;
            destinationAccount.Address = this.Address;
            destinationAccount.City = this.City;
            destinationAccount.CreditRoomd = this.CreditRoomd;
            destinationAccount.EntityId = this.EntityId;
            destinationAccount.ExpDate = this.ExpDate;
            destinationAccount.ExtensionData = this.ExtensionData;
            destinationAccount.FirstName = this.FirstName;
            destinationAccount.LastName = this.LastName;
            destinationAccount.LoginEmail = this.LoginEmail;
            destinationAccount.State = this.State;
            destinationAccount.ZipCode = this.ZipCode;

        }

        public int OwnerAccountId
        {
            get
            {
                return AccountId;
            }
        }
    }
}
