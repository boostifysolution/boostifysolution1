var userSignUp = new function () {
    var vm = this;

    vm.name = ko.observable().extend({
        required: {
            message: "Name is required"
        }
    });
    vm.email = ko.observable().extend({
        required: {
            message: "Email is required"
        }
    });
    vm.phoneNumber = ko.observable().extend({
        required: {
            message: "Phone Number is required"
        }
    });
    vm.country = ko.observable().extend({
        required: {
            message: "Country is required"
        }
    });

    vm.referralCode = ko.observable().extend({
        required: {
            message: "Referral code is required"
        }
    });

    vm.password = ko.observable().extend({
        required: {
            message: "Password is required"
        }
    });

    vm.confirmPassword = ko.observable().extend({
        required: {
            message: "Passwords needs to be the same"
        }
    });

    vm.countryOptions = ko.observableArray([
        {
            text: "Malaysia",
            id: 0
        },
        {
            text: "Indonesia",
            id: 1
        },
        {
            text: "Japan",
            id: 2
        }
    ]);

    vm.errors = ko.validation.group(vm);

    vm.signUpClick = function () {
        if (vm.errors().length > 0) {
            vm.name.isModified(true);
            vm.email.isModified(true);
            vm.phoneNumber.isModified(true);
            vm.password.isModified(true);
            vm.confirmPassword.isModified(true);
            vm.referralCode.isModified(true);

            return;
        }

        if (vm.password() != vm.confirmPassword()) {
            Swal.fire({
                icon: 'infor',
                title: 'Passwords needs to be the same',
                showConfirmButton: true,
            })

            return;
        }

        var data = {
            name: vm.name(),
            email: vm.email(),
            phoneNumber: vm.phoneNumber(),
            country: vm.country(),
            referralCode: vm.referralCode(),
            password: vm.password()
        };

        window.Global.ShowLoadingSpinner();

        $.ajax({
            url: "/api/Users/SignUp",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: 'POST',
            data: JSON.stringify(data)
        }).done(function (response) {
            window.localStorage.setItem("user", JSON.stringify(response.data));

            Swal.fire({
                icon: 'success',
                title: 'Account Created',
                text: "Redirecting to user page",
                showConfirmButton: true,
            }).then((result) => {
                window.location.href = "/users/home?culture=" + response.data.language;
            })

            setTimeout(function () {
                window.location.href = "/users/home?culture=" + response.data.language;
            }, 3000)
        }).fail(function (response) {
            window.Global.HideLoadingSpinner();

            vm.errorHandling("Failed to submit application", response);
        });
    }



    window.pageViewModel = vm;

    return vm;
}