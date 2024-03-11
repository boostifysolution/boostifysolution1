var userSignIn = new function () {
    var vm = this;

    vm.password = ko.observable().extend({
        minLength: {
            params: 8,
            message: "Passwords needs to be at least 8 characters"
        },
        required: {
            message: "Password is required"
        }
    });

    vm.username = ko.observable().extend({
        required: {
            message: "Username is required"
        }
    });

    vm.errorMessage = ko.observable();
    vm.errorMessageClass = ko.observable("d-none");

    vm.errors = ko.validation.group(vm);

    vm.initialize = function () {
        var url = new URL(window.location);

        token = url.searchParams.get("token");

        if (token != null) {
            vm.viewType(2);
        }
    }

    vm.signInClick = function () {
        if (vm.errors().length > 0) {
            vm.username.isModified(true);
            vm.password.isModified(true);

            return;
        }

        var data = {
            username: vm.username(),
            password: vm.password()
        };


        window.Global.ShowLoadingSpinner();

        $.ajax({
            url: "/api/Users/SignIn",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: 'POST',
            data: JSON.stringify(data)
        }).done(function (response) {
            window.localStorage.setItem("user", JSON.stringify(response.data));

            window.location.href = "/Users/Home?culture=" + response.data.language;
        }).fail(function (response) {
            window.Global.HideLoadingSpinner();

            showErrorMessage(response);
        });
    }

    vm.passwordEnter = function (data, event) {
        if (event.keyCode == 13) {
            if (vm.password().length >= 8) {
                vm.signInClick();
            } else {
                Swal.fire({
                    icon: 'error',
                    text: 'Passwords needs to be at least 8 characters',
                    showConfirmButton: false,
                    timer: 2000
                });
            }
        }
    }

    vm.forgetPasswordClick = function () {
        vm.email(null);
        vm.viewType(1);
    }

    vm.backClick = function () {
        vm.email(null);
        vm.viewType(0);
    }

    function showErrorMessage(response) {
        vm.errorMessageClass("d-block text-center");
        vm.errorMessage(response.responseJSON.error.message);
    }

    window.pageViewModel = vm;

    return vm;
}