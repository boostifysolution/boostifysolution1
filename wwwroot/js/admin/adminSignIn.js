var adminSignIn = new function () {
    var vm = this;

    vm.viewType = ko.observable(0);
    vm.email = ko.observable();
    vm.newPassword = ko.observable();
    vm.newPasswordConfirm = ko.observable()
    var token;

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
            url: "/api/Admin/SignIn",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: 'POST',
            data: JSON.stringify(data)
        }).done(function (response) {
            window.localStorage.setItem("admin", JSON.stringify(response.data));

            if (response.data.requirePasswordChange) {
                window.location.href = "/admin/users?rpc=true";
            } else {
                window.location.href = "/admin/users";
            }
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

    vm.resetPasswordClick = function () {
        if (vm.email() == null) {
            Swal.fire({
                icon: 'info',
                text: 'Email is required',
                showConfirmButton: false,
                timer: 2000
            });
        }

        window.Global.ShowLoadingSpinner();

        var data = {
            username: vm.email()
        }

        $.ajax({
            url: "/api/Admin/ForgetPassword",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: 'PUT',
            data: JSON.stringify(data)
        }).done(function (response) {
            window.Global.HideLoadingSpinner();

            Swal.fire({
                icon: 'success',
                title: 'Reset Password email sent',
                text: "Follow the instructions in the email to reset your password",
                showConfirmButton: true,
            }).then((result) => {
                if (result.isConfirmed) {
                    vm.backClick();
                } else if (result.isDenied) {
                    vm.backClick();
                }
            })
        }).fail(function (response) {
            window.Global.HideLoadingSpinner();

            Swal.fire({
                icon: 'error',
                title: 'Password Reset Failed',
                text: response.responseJSON.error.message,
                showConfirmButton: true,
            })
        });
    }

    vm.updatePasswordClick = function () {
        if (vm.newPassword() == null || vm.newPasswordConfirm() == null) {
            Swal.fire({
                icon: 'info',
                text: 'Password is required',
                showConfirmButton: false,
                timer: 2000
            });

            return;
        }

        if (vm.newPassword() != vm.newPasswordConfirm()) {
            Swal.fire({
                icon: 'info',
                text: 'Passwords need to match',
                showConfirmButton: false,
                timer: 2000
            });

            return;
        }

        window.Global.ShowLoadingSpinner();

        var data = {
            newPassword: vm.newPassword(),
            newPasswordConfirm: vm.newPasswordConfirm(),
            token: token
        }

        $.ajax({
            url: "/api/Admin/UpdatePassword",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: 'PUT',
            data: JSON.stringify(data)
        }).done(function (response) {
            window.Global.HideLoadingSpinner();

            Swal.fire({
                icon: 'success',
                title: 'Successfully updated password',
                text: "You may log in to your account with your new password",
                showConfirmButton: true,
            }).then((result) => {
                if (result.isConfirmed) {
                    vm.backClick();
                } else if (result.isDenied) {
                    vm.backClick();
                }
            })
        }).fail(function (response) {
            window.Global.HideLoadingSpinner();

            Swal.fire({
                icon: 'error',
                title: 'Password Reset Failed',
                text: response.responseJSON.error.message,
                showConfirmButton: true,
            })
        });
    }

    function showErrorMessage(response) {
        vm.errorMessageClass("d-block text-center");
        vm.errorMessage(response.responseJSON.error.message);
    }

    window.pageViewModel = vm;

    return vm;
}