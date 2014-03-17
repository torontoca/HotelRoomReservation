using Core.Common.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomReservation.Client.Entities
{
    public class Reservation : ObjectBase
    {
        private int _reservationId;
        public int ReservationId
        {
            get { return _reservationId; }
            set
            {
                if (_reservationId == value) return;

                _reservationId = value;
                OnPropertyChanged(() => ReservationId);
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
                OnPropertyChanged(() => AccountId);
            }
        }

        private int _roomId;
        public int RoomId
        {
            get { return _roomId; }
            set
            {
                if(_roomId == value) return;

                _roomId = value;
                OnPropertyChanged(() => RoomId);
            }
        }

        private DateTime _returnDate;
        public DateTime ReturnDate
        {
            get { return _returnDate; }
            set
            {
                if (_returnDate == value) return;

                _returnDate = value;
                OnPropertyChanged(() => ReturnDate);
            }
        }

        private DateTime _rentalDate;
        public DateTime RentalDate
        {
            get { return _rentalDate; }
            set
            {
                if (_rentalDate == value) return;

                _rentalDate = value;
                OnPropertyChanged(() => RentalDate);
            }
        }
    }
}
