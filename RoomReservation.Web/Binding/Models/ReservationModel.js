
(function (cr) {
    var ReservationModel = function (reservationId, room, rentalDate, returnDate) {

        var self = this;

        self.ReservationId = ko.observable(reservationId);
        self.Room = ko.observable(room);
        self.RentalDate = ko.observable(rentalDate);
        self.ReturnDate = ko.observable(returnDate);

        self.CancelRequest = ko.observable(false);
    };
    cr.ReservationModel = ReservationModel;
}(window.RoomRental));
