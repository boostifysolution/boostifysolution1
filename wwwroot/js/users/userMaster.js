var userMaster = new function () {
    var vm = this;

    vm.pageType = ko.observable();
    vm.userName = ko.observable();
    vm.userId = ko.observable();
    vm.currency = ko.observable();
    vm.accountStatus = ko.observable();
    vm.language = ko.observable();

    vm.masterInitialize = function () {
        var url = new URL(window.location);
        demo = url.searchParams.get("demo");

        var userJSON = window.localStorage.getItem("user");
        if (userJSON != null) {
            var userDetails = JSON.parse(userJSON);
            vm.userName(userDetails.userName);
            vm.userId(userDetails.userId);
            vm.currency(userDetails.currency);
            vm.accountStatus(userDetails.accountStatus);
            vm.language(userDetails.language);
        } else if (demo == null || demo != "true") {
            Swal.fire({
                icon: "error",
                title: "Session Expired",
                text: "Please sign in again.",
                showConfirmButton: false,
                timer: 1500,
            });

            setTimeout(function () {
                window.location.href = "/users/signin";
            }, 1500)
        }


        if (url.pathname.toLowerCase().includes('/users/home')) {
            vm.pageType(0);
        } else if (url.pathname.toLowerCase().includes('/users/wallet')) {
            vm.pageType(1);
        } else if (url.pathname.toLowerCase().includes('/users/profile')) {
            vm.pageType(2);
        }
    }

    vm.signOutClick = function () {
        $.ajax({
            url: "/api/Users/SignOut/",
            processData: false,
            contentType: false,
            type: 'POST'
        }).done(function (response) {
            window.localStorage.removeItem("user");

            window.location.href = "/users/signin";

        }).fail(function (response) {
            window.localStorage.removeItem("user");

            window.location.href = "/users/signin";

        });
    }

    vm.errorHandling = function (title, response) {
        if (response.status == 401) {
            Swal.fire({
                icon: "error",
                title: "Unauthorized",
                text: "Session Expired. Please sign in again.",
                showConfirmButton: false,
                timer: 2500,
            });

            setTimeout(function () {
                window.location.href = "/users/signin";
            }, 2500)
        } else if (response.status == 400) {
            Swal.fire({
                icon: "error",
                title: title,
                text: response.responseJSON.error.message,
                showConfirmButton: true,
            });
        }
        else {
            Swal.fire({
                icon: "error",
                title: title,
                text: response.responseJSON.data,
                showConfirmButton: true,
            });
        }
    }

    vm.redirectPage = function (page) {
        window.location.href = page + "?culture=" + vm.language();
    }

    window.pageViewModel = vm;

    return vm;
}
