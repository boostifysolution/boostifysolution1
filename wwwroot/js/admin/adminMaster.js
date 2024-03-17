var adminMaster = new function () { //rename this to the file name
    var vm = this;

    vm.pageType = ko.observable();
    vm.currentAdminStaffId = ko.observable();
    vm.adminStaffType = ko.observable();
    vm.adminStaffEmail = ko.observable();

    vm.masterInitialize = function () {
        var url = new URL(window.location);

        var adminJSON = window.localStorage.getItem("admin");
        if (adminJSON != null) {
            var adminDetails = JSON.parse(adminJSON);
            vm.currentAdminStaffId(adminDetails.adminStaffId);
            vm.adminStaffType(adminDetails.adminStaffType);
            vm.adminStaffEmail(adminDetails.adminStaffEmail);
        } else {
            Swal.fire({
                icon: "error",
                title: "Session Expired",
                text: "Please sign in again.",
                showConfirmButton: false,
                timer: 1500,
            });

            setTimeout(function () {
                window.location.href = "/admin/signin";
            }, 1500)
        }
    }

    vm.signOutClick = function () {
        $.ajax({
            url: "/api/Admin/SignOut/",
            processData: false,
            contentType: false,
            type: 'POST'
        }).done(function (response) {
            window.localStorage.removeItem("admin");

            window.location.href = "/admin/signin";

        }).fail(function (response) {
            window.localStorage.removeItem("admin");

            window.location.href = "/admin/signin";

        });
    }

    vm.errorHandling = function (title, response) {
        if (response.status == 401) {
            Swal.fire({
                icon: "error",
                title: "Session Expired",
                text: "Please sign in again.",
                showConfirmButton: false,
                timer: 1500,
            });

            setTimeout(function () {
                window.location.href = "/admin/signin";
            }, 1500)
        } else {
            Swal.fire({
                icon: "error",
                title: title,
                text: response.responseJSON.data,
                showConfirmButton: true,
            });
        }
    }

    window.pageViewModel = vm;

    return vm;
}
