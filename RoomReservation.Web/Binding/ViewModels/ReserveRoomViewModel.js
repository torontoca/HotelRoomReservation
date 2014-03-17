
(function (cr) {
    var ReserveRoomViewModel = function () {

        var self = this;

        var initialState = 'reserve';

        self.viewModelHelper = new RoomRental.viewModelHelper();
        self.viewMode = ko.observable(); // reserve, roomlist, success
        self.reservationModel = ko.observable();
        self.rooms = ko.observableArray();
        self.reservationNumber = ko.observable();

        var pickupDate = null;
        var returnDate = null;

        self.initialize = function () {
            self.reservationModel(new RoomRental.ReserveRoomModel());
            self.viewMode('reserve');
        };
        self.availableRooms = function (model) {
            var errors = ko.validation.group(model, { deep: true });
            self.viewModelHelper.modelIsValid(model.isValid());
            if (errors().length == 0) {
                self.viewModelHelper.apiGet('api/reservation/availablerooms',
                    { pickupDate: model.PickupDate(), returnDate: model.ReturnDate() },
                    function (result) {
                        pickupDate = model.PickupDate();
                        returnDate = model.ReturnDate();
                        ko.mapping.fromJS(result, {}, self.rooms);
                        self.viewMode('roomlist');
                    });
            }
            else
                self.viewModelHelper.modelErrors(errors());

        };
        self.selectRoom = function (room) {
            var model = { PickupDate: pickupDate, ReturnDate: returnDate, Room: room.RoomId() };
            self.viewModelHelper.apiPost('api/reservation/reserveroom', model,
                function (result) {
                    self.reservationNumber(result.ReservationId);
                    self.viewMode('success');
                });
        };
        self.reservationDates = function () {
            self.rooms([]);
            self.viewMode('reserve');
        };
        self.viewMode.subscribe(function () {
            switch (self.viewMode()) {
                case 'reserve':
                    self.viewModelHelper.pushUrlState('reserve', null, null, 'customer/reserve');
                    break;
                case 'roomlist':
                    self.viewModelHelper.pushUrlState('roomlist', null, null, 'customer/reserve');
                    break;
            }

            initialState = self.viewModelHelper.handleUrlState(initialState);
        });

        if (Modernizr.history) {
            window.onpopstate = function (arg) {
                if (arg.state != null) {
                    self.viewModelHelper.statePopped = true;
                    self.viewMode(arg.state.Code);
                }
            };
        }

        self.initialize();
    };
    cr.ReserveRoomViewModel = ReserveRoomViewModel;
}(window.RoomRental));
