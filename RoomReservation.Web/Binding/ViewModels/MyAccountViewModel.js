
(function (cr) {
    var MyAccountViewModel = function () {

        var self = this;

        self.viewModelHelper = new RoomRental.viewModelHelper();
        self.viewMode = ko.observable(); // account, success
        self.accountModel = ko.observable();

        self.initialize = function () {
            self.viewModelHelper.apiGet('api/customer/getaccount', null,
                function (result) {
                    self.accountModel(new RoomRental.MyAccountModel(result.AccountId, result.LoginEmail,
                                                                   result.FirstName, result.LastName,
                                                                   result.Address, result.City, result.State, 
                                                                   result.ZipCode, result.CreditRoomd, result.ExpDate));
                    self.viewMode('account');
                });
        };
        self.save = function (model) {
            var errors = ko.validation.group(model, { deep: true });
            self.viewModelHelper.modelIsValid(model.isValid());
            if (errors().length == 0) {
                var unmappedModel = ko.mapping.toJS(model);
                self.viewModelHelper.apiPost('api/customer/updateaccount', unmappedModel,
                    function (result) {
                        self.viewMode('success');
                    });
            }
            else
                self.viewModelHelper.modelErrors(errors());
        };
        self.initialize();
    };
    cr.MyAccountViewModel = MyAccountViewModel;
}(window.RoomRental));
