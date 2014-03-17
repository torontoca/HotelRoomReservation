using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Core.Common.Core;

namespace RoomReservation.Client.Entities
{
    public class Rental : ObjectBase
    {
        private int _rentalId;
        public int RentalId
        {
            get { return _rentalId; }
            set
            {
                if (_rentalId == value) return;

                _rentalId = value;
                OnPropertyChanged(() => RentalId);
            }
        }

        private int _accountId;
        public int AccountId
        {
            get { return _accountId; }
            set
            {
                if (_accountId == value) return;

                _accountId = value;
                OnPropertyChanged( () => AccountId);
            }
        }

        private int _roomId;
        public int RoomId
        {
            get { return _roomId; }
            set
            {
                if (_roomId == value) return;

                _roomId = value;
                OnPropertyChanged(() => RoomId);
            }
        }

        private DateTime _dateRented;
        public DateTime DateRented
        {
            get { return _dateRented; }
            set
            {
                if (_dateRented == value) return;

                _dateRented = value;
                OnPropertyChanged(() => DateRented);
            }
        }

        private DateTime _dateDue;
        public DateTime DateDue
        {
            get { return _dateDue; }
            set
            {
                if (_dateDue == value) return;

                _dateDue = value;
                OnPropertyChanged(() => DateDue);
            }
        }

        private DateTime? _dateReturned;
        public DateTime? DateReturned
        {
            get { return _dateReturned; }
            set
            {
                if (_dateReturned == null)
                {
                    _dateReturned = value;
                    return;
                }

                if (_dateReturned == value) return;

                _dateReturned = value;
                OnPropertyChanged(() => DateReturned);
            }
        }
    }
}
