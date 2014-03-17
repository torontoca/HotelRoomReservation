using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Core;
using FluentValidation;

namespace RoomReservation.Client.Entities
{
    public class Room : ObjectBase
    {
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

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                if (_description == value) return;

                _description = value;
                OnPropertyChanged( () => Description);
            }
        }

        private string _color;
        public string Color
        {
            get { return _color; }
            set
            {
                if (_color == value) return;

                _color = value;
                OnPropertyChanged(() => Color);
            }
        }

        private int _year;
        public int Year
        {
            get { return _year; }
            set
            {
                if (_year == value) return;

                _year = value;
                OnPropertyChanged(() => Year);
            }
        }

        private decimal _rentalPrice;
        public decimal RentalPrice
        {
            get { return _rentalPrice; }
            set
            {
                if (_rentalPrice == value) return;

                _rentalPrice = value;
                OnPropertyChanged(() => RentalPrice);
            }
        }

        private bool _currentlyRented;
        public bool CurrentlyRented
        {
            get { return _currentlyRented; }
            set
            {
                if (_currentlyRented == value) return;

                _currentlyRented = value;
                OnPropertyChanged(() => CurrentlyRented);
            }
        }


        private class RoomValidator : AbstractValidator<Room>
        {
            public RoomValidator()
            {
                RuleFor(obj => obj.Description).NotEmpty();
                RuleFor(obj => obj.Color).NotEmpty();
                RuleFor(obj => obj.RentalPrice).GreaterThan(0);
                RuleFor(obj => obj.Year).GreaterThan(2000).LessThanOrEqualTo(DateTime.Now.Year);
            }
        }

        protected override IValidator GetValidator()
        {
            return new RoomValidator();
        }
    }
}
