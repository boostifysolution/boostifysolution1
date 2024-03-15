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
            text: "India",
            id: 2
        }
    ]);

    vm.errors = ko.validation.group(vm);

    vm.signUpClick = function () {
        if (vm.errors().length > 0) {
            vm.name.isModified(true);
            vm.email.isModified(true);

            return;
        }

        var data = {
            name: vm.name(),
            email: vm.email(),
            phoneNumber: vm.phoneNumber(),
            country: vm.country()
        };

        window.Global.ShowLoadingSpinner();

        $.ajax({
            url: "/api/Users/SignUp",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: 'POST',
            data: JSON.stringify(data)
        }).done(function (response) {
            Swal.fire({
                icon: 'success',
                title: 'Application Submitted',
                text: "Our admin will be in touch soon. Thank you for your interest",
                showConfirmButton: true,
            }).then((result) => {
                window.location.href = "/";
            })
        }).fail(function (response) {
            window.Global.HideLoadingSpinner();

            vm.errorHandling("Failed to submit application", response);
        });
    }



    window.pageViewModel = vm;

    return vm;
}