
var adminStaffs = new function () {
    var vm = window.pageViewModel;

    vm.adminStaffId = ko.observable();
    vm.name = ko.observable();
    vm.password = ko.observable();
    vm.loginUsername = ko.observable();
    vm.adminType = ko.observable();
    vm.adminTypeOptions = ko.observableArray();
    vm.status = ko.observable();
    vm.accounts = ko.observable();
    vm.adminLeaderOptions = ko.observableArray();
    vm.adminLeaderStaffId = ko.observable();
    vm.referralCode = ko.observable();
    vm.firstTaskId = ko.observable();
    vm.taskOptions = ko.observableArray();

    vm.filterAdminLeader = ko.observable();
    vm.filterAdminType = ko.observable();


    var usersList;

    var dataFilters;

    vm.initialize = function () {
        loadStaffs();
    }

    vm.applyFilters = function () {
        dataFilters = {
            filterAdminLeader: vm.filterAdminLeader(),
            filterAdminType: vm.filterAdminType()
        };

        $("#staffsGrid").jsGrid("loadData");
    }

    vm.clearFilters = function () {
        vm.filterAdminLeader(null);
        vm.filterAdminType(null);

        vm.applyFilters();
    }

    vm.addNewStaffClick = function () {
        vm.name(null);
        vm.loginUsername(null);
        vm.password("tempPassword!23");
        vm.adminType(null);

        MicroModal.show("new-staff-details-modal");
    }

    vm.saveStaffDetails = function () {
        window.Global.ShowLoadingSpinner();

        var data = {
            name: vm.name(),
            referralCode: vm.referralCode(),
            firstTaskId: vm.firstTaskId(),
        }

        $.ajax({
            url: "/api/Admin/Staffs/" + vm.adminStaffId(),
            dataType: "json",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            type: "PUT",
        })
            .done(function (response) {
                Swal.fire({
                    icon: "success",
                    title: "Staff Details Updated",
                    showConfirmButton: true,
                }).then((result) => {
                    MicroModal.close("staff-details-modal");
                })

                $("#staffsGrid").jsGrid("loadData");
            })
            .fail(function (response) {
                window.Global.HideLoadingSpinner();

                vm.errorHandling("Failed to update staff details", response);
            });
    }

    vm.saveNewStaffClick = function () {
        window.Global.ShowLoadingSpinner();

        var data = {
            name: vm.name(),
            loginUsername: vm.loginUsername(),
            password: vm.password(),
            adminType: vm.adminType(),
            adminLeaderStaffId: vm.adminLeaderStaffId()
        }

        $.ajax({
            url: "/api/Admin/Staffs",
            dataType: "json",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            type: "POST",
        })
            .done(function (response) {
                Swal.fire({
                    icon: "success",
                    title: "New Staff Created",
                    showConfirmButton: true,
                }).then((result) => {
                    MicroModal.close("new-staff-details-modal");
                })

                $("#staffsGrid").jsGrid("loadData");
            })
            .fail(function (response) {
                window.Global.HideLoadingSpinner();

                vm.errorHandling("Failed to create new staff", response);
            });
    }


    function loadStaffs() {
        window.Global.ShowLoadingSpinner();

        $("#staffsGrid").jsGrid({
            autoload: true,
            width: "100%",
            height: "auto",
            inserting: false,
            editing: false,
            sorting: true,
            paging: true,
            filtering: false,
            pageLoading: true,
            pageIndex: 1,
            pageSize: 25,
            noDataContent: "No admins yet",
            rowClick: function (args) {
                staffClick(args.item.adminStaffId);
            },
            fields: [
                {
                    name: "name",
                    type: "text",
                    title: "Name",
                    width: 100
                },
                {
                    name: "email",
                    type: "text",
                    title: "Email",
                    width: 150
                },
                {
                    name: "leader",
                    type: "text",
                    title: "Leader",
                    width: 100
                },
                {
                    name: "status",
                    type: "text",
                    title: "Status",
                    width: 80
                },
                {
                    name: "adminType",
                    type: "text",
                    title: "Admin Type",
                    width: 80,
                },
                {
                    name: "dateAdded",
                    type: "text",
                    title: "Date Added",
                    width: 80,
                }
            ],
            controller: {
                loadData: function (gridFilters) {
                    var d = $.Deferred();

                    var filters = {
                        ...gridFilters,
                        ...dataFilters,
                    };

                    $.ajax({
                        url: "/api/Admin/Staffs",
                        dataType: "json",
                        data: filters,
                        contentType: "application/json; charset=utf-8",
                        type: "GET",
                    })
                        .done(function (response) {
                            var da = {
                                data: response.data.staffsList,
                                itemsCount: response.data.itemsCount,
                            };

                            vm.adminLeaderOptions(response.data.adminLeaderOptions);
                            vm.adminTypeOptions(response.data.adminTypeOptions);
                            vm.taskOptions(response.data.taskOptions);

                            d.resolve(da);
                            window.Global.HideLoadingSpinner();
                        })
                        .fail(function (response) {
                            window.Global.HideLoadingSpinner();

                            vm.errorHandling("Failed to load admin staff", response);
                        });

                    return d.promise();
                },
            },
        });
    }

    function staffClick(staffId) {
        vm.adminStaffId(staffId);

        MicroModal.show("staff-details-modal");

        loadStaffDetails();
    }

    function loadStaffDetails() {
        window.Global.ShowLoadingSpinner();

        $.ajax({
            url: "/api/Admin/Staffs/" + vm.adminStaffId(),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "GET",
        })
            .done(function (response) {
                window.Global.HideLoadingSpinner();

                setStaffDetails(response.data)
            })
            .fail(function (response) {
                window.Global.HideLoadingSpinner();

                vm.errorHandling("Failed to load staff details", response);
            });
    }

    function setStaffDetails(responseData) {
        vm.name(responseData.name);
        vm.loginUsername(responseData.loginUsername);
        vm.adminType(responseData.adminType);
        vm.status(responseData.status);
        vm.accounts(responseData.accounts);
        vm.adminLeaderStaffId(responseData.adminLeaderStaffId);
        vm.firstTaskId(responseData.firstTaskId);
        vm.referralCode(responseData.referralCode);

        $("#staffUsersGrid").jsGrid({
            autoload: false,
            width: "100%",
            height: "auto",
            inserting: false,
            editing: false,
            sorting: false,
            paging: false,
            filtering: false,
            pageLoading: false,
            pageIndex: 1,
            pageSize: 25,
            data: responseData.usersList,
            fields: [
                {
                    name: "name",
                    type: "text",
                    title: "Name",
                    width: 150
                },
                {
                    name: "walletAmount",
                    type: "text",
                    title: "Wallet Amount",
                    width: 100
                },
                {
                    name: "dateAdded",
                    type: "text",
                    title: "Date Added",
                    width: 100,
                },
            ],
        });
    }

    vm.resetPasswordClick = function () {
        Swal.fire({
            icon: "info",
            title: "Are you sure you want to reset password?",
            text: "Password will be set to: tempPassword123",
            showConfirmButton: true,
        }).then((result) => {
            resetPassword();
        })
    }

    function resetPassword() {
        window.Global.ShowLoadingSpinner();

        $.ajax({
            url: "/api/Admin/AdminStaffs/" + vm.adminStaffId() + "/ResetPassword",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: 'PUT'
        }).done(function (response) {
            window.Global.HideLoadingSpinner();

            Swal.fire({
                icon: 'success',
                title: 'Successfully reset password',
                text: "The user can now login with new password",
                showConfirmButton: true,
            })

        }).fail(function (response) {
            window.Global.HideLoadingSpinner();

            vm.errorHandling("Password Reset Failed", response);
        });
    }

    vm.disableAccountClick = function () {

    }

    window.pageViewModel = vm;

    return vm;
}