
var adminUserLeads = new function () {
    var vm = window.pageViewModel;

    vm.userLeadId = ko.observable();
    vm.password = ko.observable().extend({
        required: {
            message: "Password is required"
        }
    });
    vm.adminStaffId = ko.observable().extend({
        required: {
            message: "Admin is required"
        }
    });
    vm.language = ko.observable().extend({
        required: {
            message: "Language is required"
        }
    });
    vm.walletAmount = ko.observable();

    vm.filterLeadStatus = ko.observable(0);
    vm.filterDateAdded = ko.observable();
    vm.filterDateAddedStart = ko.observable();
    vm.filterDateAddedEnd = ko.observable();
    vm.filterLeadName = ko.observable();

    vm.dateFilterOptions = ko.observableArray();
    vm.languageOptions = ko.observableArray();
    vm.adminOptions = ko.observableArray();
    vm.leadStatusOptions = ko.observable();

    vm.errors = ko.validation.group(vm);

    var dataFilters;

    vm.initialize = function () {
        dataFilters = {
            filterLeadStatus: vm.filterLeadStatus()
        };

        loadUserLeads();
    }

    vm.convertLeadToUserClick = function () {
        if (selectedUserLeads.length == 0) {
            Swal.fire({
                icon: "info",
                title: "Please select at least 1 user lead to be converted",
                showConfirmButton: true,
            });

            return;
        }

        vm.password("tempPassword!23");
        vm.adminStaffId(null);
        vm.walletAmount(null);
        vm.language(null);

        MicroModal.show("convert-lead-to-user-details");
    }

    vm.applyFilters = function () {
        dataFilters = {
            filterLeadStatus: vm.filterLeadStatus(),
            filterLeadName: vm.filterLeadName(),
            filterDateAdded: vm.filterDateAdded(),
            filterDateAddedStart: vm.filterDateAddedStart(),
            filterDateAddedEnd: vm.filterDateAddedEnd(),
        };

        $("#userLeadsGrid").jsGrid("loadData");
    }

    vm.clearFilters = function () {
        vm.filterLeadStatus(null);
        vm.filterLeadName(null);
        vm.filterDateAdded(null);
        vm.filterDateAddedStart(null);
        vm.filterDateAddedEnd(null);

        vm.applyFilters();
    }

    vm.closeUserLeadDetailsModal = function () {
        MicroModal.close("convert-lead-to-user-details");
    }

    var selectedUserLeads = [];

    function loadUserLeads() {
        window.Global.ShowLoadingSpinner();

        $("#userLeadsGrid").jsGrid({
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
            noDataContent: "No user leads yet",
            fields: [
                {
                    title: "Select",
                    itemTemplate: function (_, item) {
                        return $("<input>").attr("type", "checkbox")
                            .prop("checked", $.inArray(item, selectedUserLeads) > -1)
                            .on("change", function () {
                                $(this).is(":checked") ? selectItem(item) : unselectItem(item);
                            })
                            .on("click", function (e) {
                                e.stopPropagation();
                            })
                    },
                    align: "center",
                    width: 80,
                    sorting: false
                },
                {
                    name: "name",
                    type: "text",
                    title: "Lead Name",
                    width: 150
                },
                {
                    name: "phoneNumber",
                    type: "text",
                    title: "Phone Number",
                    width: 100
                },
                {
                    name: "email",
                    type: "text",
                    title: "Email",
                    width: 150,
                },
                {
                    name: "leadStatus",
                    type: "text",
                    title: "Lead Status",
                    width: 80,
                },
                {
                    name: "country",
                    type: "text",
                    title: "Country",
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
                        url: "/api/Admin/UserLeads",
                        dataType: "json",
                        data: filters,
                        contentType: "application/json; charset=utf-8",
                        type: "GET",
                    })
                        .done(function (response) {
                            var da = {
                                data: response.data.userLeadsList,
                                itemsCount: response.data.itemsCount,
                            };

                            vm.dateFilterOptions(response.data.dateFilterOptions);
                            vm.leadStatusOptions(response.data.leadStatusOptions);
                            vm.adminOptions(response.data.adminOptions);
                            vm.languageOptions(response.data.languageOptions);

                            d.resolve(da);
                            window.Global.HideLoadingSpinner();
                        })
                        .fail(function (response) {
                            window.Global.HideLoadingSpinner();

                            vm.errorHandling("Failed to load user leads", response);
                        });

                    return d.promise();
                },
            },
        });
    }

    vm.convertToUserSubmit = function () {
        window.Global.ShowLoadingSpinner();

        var data = {
            userLeadIds: selectedUserLeads,
            password: vm.password(),
            adminStaffId: vm.adminStaffId(),
            language: vm.language(),
            walletAmount: vm.walletAmount(),
        }

        $.ajax({
            url: "/api/Admin/UserLeads/ConvertToUser",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "PUT",
            data: JSON.stringify(data)
        })
            .done(function (response) {
                window.Global.HideLoadingSpinner();

                Swal.fire({
                    icon: "success",
                    title: "Successfully Converted Lead to User",
                    showConfirmButton: true,
                });

                MicroModal.close("convert-lead-to-user-details");

                $("#userLeadsGrid").jsGrid("loadData");
            })
            .fail(function (response) {
                window.Global.HideLoadingSpinner();

                vm.errorHandling("Failed to load users", response);
            });
    }

    function selectItem(item) {
        selectedUserLeads.push(item.userLeadId);
    };

    function unselectItem(item) {
        selectedUserLeads = $.grep(selectedUserLeads, function (i) {
            return i !== item.userLeadId;
        });
    };
    window.pageViewModel = vm;

    return vm;
}