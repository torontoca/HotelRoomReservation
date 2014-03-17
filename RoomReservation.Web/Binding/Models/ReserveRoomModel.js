
(function (cr) {
    var ReserveRoomModel = function() {

        var self = this;

        self.PickupDate = ko.observable("").extend(
            { required: { message: "Pickup date is required" } }
        );
        self.ReturnDate = ko.observable("").extend(
            { required: { message: "Return date is required" } }
        );
    };
    cr.ReserveRoomModel = ReserveRoomModel;
}(window.RoomRental));
