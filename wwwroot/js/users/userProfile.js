var userProfile = new function () {
    var vm = window.pageViewModel;

    vm.fullName = ko.observable();
    vm.dateJoined = ko.observable();
    vm.phoneNumber = ko.observable();
    vm.email = ko.observable();
    vm.language = ko.observable();
    vm.languageOptions = ko.observableArray();
    vm.accountAdmin = ko.observable();
    vm.accountStatus = ko.observable();
    vm.currency = ko.observable();


    vm.initialize = function () {
        loadProfileDetails();
    }

    function loadProfileDetails() {
        window.Global.ShowLoadingSpinner();

        $.ajax({
            url: "/api/Users/" + vm.userId() +"/Profile",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "GET",
        })
            .done(function (response) {
                window.Global.HideLoadingSpinner();

                setProfileDetails(response.data);
            })
            .fail(function (response) {
                window.Global.HideLoadingSpinner();

                vm.errorHandling("Failed to load profile details", response);
            });
    }

    function setProfileDetails(responseData) {
        vm.fullName(responseData.fullName);
        vm.dateJoined(responseData.dateJoined);
        vm.phoneNumber(responseData.phoneNumber);
        vm.email(responseData.email);
        vm.language(responseData.language);
        vm.languageOptions(responseData.languageOptions);
        vm.accountAdmin(responseData.accountAdmin);
        vm.accountStatus(responseData.accountStatus);
        vm.currency(responseData.currency);
    }

    vm.saveProfileClick = function () {
        var data = {
            fullName: vm.fullName(),
            phoneNumber: vm.phoneNumber(),
            language: vm.language(),
            email: vm.email()
        };

        window.Global.ShowLoadingSpinner();

        $.ajax({
            url: "/api/Users/Profile",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: 'PUT',
            data: JSON.stringify(data)
        }).done(function (response) {
            window.Global.HideLoadingSpinner();

            Swal.fire({
                icon: "success",
                title: "User Profile Updated",
                showConfirmButton: true,
            });

        }).fail(function (response) {
            window.Global.HideLoadingSpinner();

            vm.errorHandling("Failed to save user profile", response);
        });
    }



    window.pageViewModel = vm;

    return vm;
}