

(function (cr) {

    var AccountLoginViewModel = function (returnUrl) {

        var self = this;

        self.viewModelHelper = new RoomRental.viewModelHelper();

        self.accountModel = new RoomRental.AccountLoginModel();

        self.login = function(model) {
            var errors = ko.validation.group(model);
            self.viewModelHelper.modelIsValid(model.isValid());
            if (errors().length == 0) {
                var unmappedModel = ko.mapping.toJS(model);
                self.viewModelHelper.apiPost('api/account/login', unmappedModel,
                    function(resultUrl) {
                        if (resultUrl != '' && returnUrl.length > 1)
                            window.location.href = RoomRental.rootPath + returnUrl.substring(1);
                        else
                            window.location.href = RoomRental.rootPath;
                    });

            } else {
                self.viewModelHelper.modelErrors(errors());
            }
                
        };

    };

    cr.AccountLoginViewModel = AccountLoginViewModel;

})(window.RoomRental = window.RoomRental || {})