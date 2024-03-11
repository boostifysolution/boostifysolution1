
var adminStaffs = new function () {
    var vm = window.pageViewModel;

    vm.adminStaffId = ko.observable();
    vm.name = ko.observable();
    vm.password = ko.observable();
    vm.loginUsername = ko.observable();
    vm.adminType = ko.observable();
    vm.adminTypeOptions = ko.observableArray([{ text: "Staff", id: 0 }, { text: "Full Admin", id: 1 }, { text: "Leader", id: 2 }]);
    vm.countryOptions = ko.observableArray([{ text: "Malaysia", id: 0 }, { text: "Indonesia", id: 1 }, { text: "India", id: 2 }]);
    vm.status = ko.observable();
    vm.accounts = ko.observable();
    vm.country = ko.observable();

    var usersList;

    vm.initialize = function () {
        loadStaffs();
    }

    vm.addNewStaffClick = function () {
        vm.name(null);
        vm.loginUsername(null);
        vm.password(null);
        vm.adminType(null);

        MicroModal.show("new-staff-details-modal");
    }

    vm.saveNewStaffClick = function () {
        window.Global.ShowLoadingSpinner();

        var data = {
            name: vm.name(),
            loginUsername: vm.loginUsername(),
            password: vm.password(),
            adminType: vm.adminType()
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
                    width: 150
                },
                {
                    name: "status",
                    type: "text",
                    title: "Status",
                    width: 100
                },
                {
                    name: "adminType",
                    type: "text",
                    title: "Admin Type",
                    width: 100,
                },
                {
                    name: "dateAdded",
                    type: "text",
                    title: "Date Added",
                    width: 100,
                }
            ],
            controller: {
                loadData: function (gridFilters) {
                    var d = $.Deferred();

                    $.ajax({
                        url: "/api/Admin/Staffs",
                        dataType: "json",
                        data: gridFilters,
                        contentType: "application/json; charset=utf-8",
                        type: "GET",
                    })
                        .done(function (response) {
                            var da = {
                                data: response.data.staffsList,
                                itemsCount: response.data.itemsCount,
                            };

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
        vm.country(responseData.country);

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

    window.pageViewModel = vm;

    return vm;
}