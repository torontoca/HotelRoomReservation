using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Core.Common.Core;

namespace RoomReservation.Client.Entities
{
    public class Account : ObjectBase
    {
        private int _accountId;
        public int AccountId
        {
            get { return _accountId; }
            set
            {
                if (_accountId == value) return;

                _accountId = value;
                OnPropertyChanged(() => AccountId);
            }
        }

        private string _loginEmail;
        public string LoginEmail
        {
            get { return _loginEmail; }
            set
            {
                if (_loginEmail == value) return;

                _loginEmail = value;
                OnPropertyChanged(() => LoginEmail);
            }
        }

        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (_firstName == value) return;

                _firstName = value;
                OnPropertyChanged(() => FirstName);
            }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (_lastName == value) return;

                _lastName = value;
                OnPropertyChanged(() => LastName);
            }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set
            {
                if (_address == value) return;

                _address = value;
                OnPropertyChanged( () => Address);
            }
        }

        private string _city;
        public string City
        {
            get { return _city; }
            set
            {
                if (_city == value) return;

                _city = value;
                OnPropertyChanged(() => City);
            }
        }

        private string _state;
        public string State
        {
            get { return _state; }
            set
            {
                if (_state == value) return;

                _state = value;
                OnPropertyChanged(() => State);
            }
        }

        private string _zipCode;
        public string ZipCode
        {
            get { return _zipCode; }
            set
            {
                if (_zipCode == value) return;

                _zipCode = value;
                OnPropertyChanged(() => ZipCode);
            }
        }

        private string _creditRoomd;
        public string CreditRoomd
        {
            get { return _creditRoomd; }
            set
            {
                if (_creditRoomd == value) return;

                _creditRoomd = value;
                OnPropertyChanged(() => CreditRoomd);
            }
        }

        private string _expDate;
        public string ExpDate
        {
            get { return _expDate; }
            set
            {
                if (_expDate == value) return;

                _expDate = value;
                OnPropertyChanged(() => ExpDate);
            }
        }
    }
}
