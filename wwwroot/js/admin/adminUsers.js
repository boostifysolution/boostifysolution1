
var adminUsers = new function () {
    var vm = window.pageViewModel;

    vm.userId = ko.observable();
    vm.name = ko.observable();
    vm.loginUsername = ko.observable();
    vm.password = ko.observable();
    vm.currency = ko.observable();
    vm.currencyOptions = ko.observableArray();
    vm.language = ko.observable();
    vm.languageOptions = ko.observableArray()
    vm.country = ko.observable();
    vm.countryOptions = ko.observableArray();
    vm.walletAmount = ko.observable();
    vm.accountStatus = ko.observable();
    vm.dateAdded = ko.observable();

    vm.showPopupMessage = ko.observable();
    vm.popupMessageTitle = ko.observable();
    vm.popupMessage = ko.observable();

    vm.viewType = ko.observable(1);
    var userTaskList;
    var walletTransactionList;

    //tasks
    vm.selectTask = ko.observable();
    vm.taskOptions = ko.observable();
    vm.commissionAmount = ko.observable();


    //wallet
    vm.amount = ko.observable();
    vm.walletTransactionType = ko.observable();
    vm.walletTransactionTypeOptions = ko.observableArray([{ text: "Deposit", id: 0 }, { text: "Withdraw", id: 1 }, { text: "Commission", id: 2 }])
    vm.customerDateAdded = ko.observable();

    //filters
    vm.filterUserName = ko.observable();
    vm.filterAccountStatus = ko.observable();
    vm.accountStatusOptions = ko.observableArray();
    vm.filterDateAdded = ko.observable();
    vm.filterDateAddedStart = ko.observable();
    vm.filterDateAddedEnd = ko.observable();
    vm.dateFilterOptions = ko.observableArray();
    vm.filterCountry = ko.observable();
    vm.filterLanguage = ko.observable();

    //password
    vm.password = ko.observable();
    vm.confirmPassword = ko.observable();

    var dataFilters;

    vm.initialize = function () {
        var url = new URL(window.location);
        var rpc = url.searchParams.get("rpc");

        if (rpc != null) {
            MicroModal.show("change-password-modal");
        }

        loadUsers();
    }

    vm.addNewUserClick = function () {
        vm.loginUsername(null);
        vm.password("tempPassword123");
        vm.currency(null);
        vm.language(null);
        vm.walletAmount(null);
        vm.userId(null);

        MicroModal.show("new-user-details-modal");
    }

    vm.saveNewUserClick = function () {
        window.Global.ShowLoadingSpinner();

        var data = {
            loginUsername: vm.loginUsername(),
            password: vm.password(),
            currency: vm.currency(),
            language: vm.language(),
            walletAmount: vm.walletAmount(),
            name: vm.name()
        }

        $.ajax({
            url: "/api/Admin/Users",
            dataType: "json",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            type: "POST",
        })
            .done(function (response) {
                Swal.fire({
                    icon: "success",
                    title: "New User Created",
                    showConfirmButton: true,
                }).then((result) => {
                    MicroModal.close("new-user-details-modal");
                })

                $("#usersGrid").jsGrid("loadData");
            })
            .fail(function (response) {
                window.Global.HideLoadingSpinner();

                vm.errorHandling("Failed to create new users", response);
            });
    }

    vm.applyFilters = function () {
        dataFilters = {
            filterUserName: vm.filterUserName(),
            filterAccountStatus: vm.filterAccountStatus(),
            filterDateAdded: vm.filterDateAdded(),
            filterDateAddedStart: vm.filterDateAddedStart(),
            filterDateAddedEnd: vm.filterDateAddedEnd(),
            filterCountry: vm.filterCountry(),
            filterLanguage: vm.filterLanguage(),
        };

        $("#usersGrid").jsGrid("loadData");
    }

    vm.clearFilters = function () {
        vm.filterUserName(null);
        vm.filterAccountStatus(null);
        vm.filterDateAdded(null);
        vm.filterDateAddedStart(null);
        vm.filterDateAddedEnd(null);
        vm.filterCountry(null);
        vm.filterLanguage(null);

        vm.applyFilters();

    }

    vm.closeUserDetailsModal = function () {
        MicroModal.close("user-details-modal");
    }

    function loadUsers() {
        window.Global.ShowLoadingSpinner();

        if (vm.adminStaffType() == 1 || vm.adminStaffType() == 2) {
            $("#usersGrid").jsGrid({
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
                noDataContent: "No users yet",
                rowClick: function (args) {
                    userClick(args.item.userId);
                },
                fields: [
                    {
                        name: "userId",
                        type: "text",
                        title: "User Id",
                        width: 50
                    },
                    {
                        name: "userName",
                        type: "text",
                        title: "Name",
                        width: 150
                    },
                    {
                        name: "wallet",
                        type: "text",
                        title: "Wallet",
                        width: 100,
                    },
                    {
                        name: "accountStatus",
                        type: "text",
                        title: "Account Status",
                        width: 100,
                    },
                    {
                        name: "adminStaffName",
                        type: "text",
                        title: "Staff",
                        width: 100,
                    },
                    {
                        name: "dateAdded",
                        type: "text",
                        title: "Date Added",
                        width: 100,
                    },
                ],
                controller: {
                    loadData: function (gridFilters) {
                        var d = $.Deferred();

                        var filters = {
                            ...gridFilters,
                            ...dataFilters,
                        };

                        $.ajax({
                            url: "/api/Admin/Users",
                            dataType: "json",
                            data: filters,
                            contentType: "application/json; charset=utf-8",
                            type: "GET",
                        })
                            .done(function (response) {
                                var da = {
                                    data: response.data.usersList,
                                    itemsCount: response.data.itemsCount,
                                };

                                vm.dateFilterOptions(response.data.dateFilterOptions);
                                vm.accountStatusOptions(response.data.accountStatusOptions);
                                vm.languageOptions(response.data.languageOptions);
                                vm.countryOptions(response.data.countryOptions);
                                vm.currencyOptions(response.data.currencyOptions);

                                d.resolve(da);
                                window.Global.HideLoadingSpinner();
                            })
                            .fail(function (response) {
                                window.Global.HideLoadingSpinner();

                                vm.errorHandling("Failed to load users", response);
                            });

                        return d.promise();
                    },
                },
            });
        } else {
            $("#usersGrid").jsGrid({
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
                noDataContent: "No users yet",
                rowClick: function (args) {
                    userClick(args.item.userId);
                },
                fields: [
                    {
                        name: "userId",
                        type: "text",
                        title: "User Id",
                        width: 50
                    },
                    {
                        name: "userName",
                        type: "text",
                        title: "Name",
                        width: 150
                    },
                    {
                        name: "wallet",
                        type: "text",
                        title: "Wallet",
                        width: 100,
                    },
                    {
                        name: "accountStatus",
                        type: "text",
                        title: "Account Status",
                        width: 100,
                    },
                    {
                        name: "dateAdded",
                        type: "text",
                        title: "Date Added",
                        width: 100,
                    },
                ],
                controller: {
                    loadData: function (gridFilters) {
                        var d = $.Deferred();

                        var filters = {
                            ...gridFilters,
                            ...dataFilters,
                        };

                        $.ajax({
                            url: "/api/Admin/Users",
                            dataType: "json",
                            data: filters,
                            contentType: "application/json; charset=utf-8",
                            type: "GET",
                        })
                            .done(function (response) {
                                var da = {
                                    data: response.data.usersList,
                                    itemsCount: response.data.itemsCount,
                                };

                                vm.dateFilterOptions(response.data.dateFilterOptions);
                                vm.accountStatusOptions(response.data.accountStatusOptions);

                                d.resolve(da);
                                window.Global.HideLoadingSpinner();
                            })
                            .fail(function (response) {
                                window.Global.HideLoadingSpinner();

                                vm.errorHandling("Failed to load users", response);
                            });

                        return d.promise();
                    },
                },
            });
        }
    }

    vm.freezeAccountClick = function () {
        window.Global.ShowLoadingSpinner();

        $.ajax({
            url: "/api/Admin/Users/" + vm.userId() + "/FreezeAccount",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "PUT",
        })
            .done(function (response) {
                window.Global.HideLoadingSpinner();

                Swal.fire({
                    icon: "success",
                    title: "Account Status Updated",
                    showConfirmButton: true,
                });

                MicroModal.close("user-details-modal");

                $("#usersGrid").jsGrid("loadData");
            })
            .fail(function (response) {
                window.Global.HideLoadingSpinner();

                vm.errorHandling("Failed to load users", response);
            });
    }

    vm.addWalletCreditClick = function () {
        vm.amount(null);
        vm.walletTransactionType(null);

        MicroModal.show("add-wallet-credit-modal");
    }


    vm.addTaskClick = function () {
        vm.selectTask(null);
        vm.commissionAmount(null);

        loadTasksList();

        MicroModal.show("add-task-modal");

    }

    vm.addPopupMessage = function () {
        var data = {
            showPopupMessage: vm.showPopupMessage(),
            popupMessageTitle: vm.popupMessageTitle(),
            popupMessage: vm.popupMessage()
        }

        window.Global.ShowLoadingSpinner();

        $.ajax({
            url: "/api/Admin/Users/" + vm.userId() + "/AddPopupMessage",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "PUT",
            data: JSON.stringify(data)
        })
            .done(function (response) {
                window.Global.HideLoadingSpinner();

                Swal.fire({
                    icon: "success",
                    title: "Added Popup Message",
                    showConfirmButton: true,
                }).then((result) => {
                    MicroModal.close("add-popup-message-modal");
                })

                loadUserDetails();
            })
            .fail(function (response) {
                window.Global.HideLoadingSpinner();

                vm.errorHandling("Failed to add popup message", response);
            });
    }

    vm.addPopupMessageClick = function () {
        MicroModal.show("add-popup-message-modal");

    }

    vm.viewTypeClick = function (value) {
        vm.viewType(value);

        if (value == 0) {
            $("#userWalletGrid").jsGrid({
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
                rowClick: function (args) {
                    updateWalletTransactionClick(args.item.status, args.item.walletTransactionId);
                },
                data: walletTransactionList,
                fields: [
                    {
                        name: "amount",
                        type: "text",
                        title: "Amount",
                        width: 100
                    },
                    {
                        name: "type",
                        type: "text",
                        title: "Type",
                        width: 100
                    },
                    {
                        name: "status",
                        type: "text",
                        title: "Status",
                        width: 100,
                    },
                    {
                        name: "dateAdded",
                        type: "text",
                        title: "Date Added",
                        width: 100,
                    },
                ],
            });
        } else {
            $("#userTasksGrid").jsGrid({
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
                data: userTaskList,
                rowClick: function (args) {
                    userTaskClick(args.item.status, args.item.userTaskId);
                },

                fields: [
                    {
                        name: "taskTitle",
                        type: "text",
                        title: "Task Title",
                        width: 100
                    },
                    {
                        name: "status",
                        type: "text",
                        title: "Status",
                        width: 50,
                    },
                    {
                        name: "commission",
                        type: "text",
                        title: "Comms",
                        width: 50
                    },
                    {
                        name: "category",
                        type: "text",
                        title: "Category",
                        width: 50,
                    },
                    {
                        name: "quantityPurchased",
                        type: "text",
                        title: "Quantity",
                        width: 50,
                    },
                    {
                        name: "dateCompleted",
                        type: "text",
                        title: "User Start",
                        width: 50,
                    },
                    {
                        name: "dateAdded",
                        type: "text",
                        title: "Added",
                        width: 50,
                    },
                ],
            });
        }
    }

    vm.saveTask = function () {
        var data = {
            taskId: vm.selectTask(),
            commission: vm.commissionAmount()
        }

        window.Global.ShowLoadingSpinner();

        $.ajax({
            url: "/api/Admin/Users/" + vm.userId() + "/AddTask",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "POST",
            data: JSON.stringify(data)
        })
            .done(function (response) {
                window.Global.HideLoadingSpinner();

                Swal.fire({
                    icon: "success",
                    title: "Task Added",
                    showConfirmButton: true,
                }).then((result) => {
                    MicroModal.close("add-task-modal");
                })

                loadUserDetails();
            })
            .fail(function (response) {
                window.Global.HideLoadingSpinner();

                vm.errorHandling("Failed to add user task", response);
            });
    }

    vm.addWalletCredit = function () {
        var data = {
            amount: vm.amount(),
            walletTransactionType: vm.walletTransactionType(),
            customDateAdded: vm.customerDateAdded()
        }

        window.Global.ShowLoadingSpinner();

        $.ajax({
            url: "/api/Admin/Users/" + vm.userId() + "/AddWalletCredit",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "POST",
            data: JSON.stringify(data)
        })
            .done(function (response) {
                window.Global.HideLoadingSpinner();

                Swal.fire({
                    icon: "success",
                    title: "Credit Added",
                    showConfirmButton: true,
                }).then((result) => {
                    MicroModal.close("add-wallet-credit-modal");
                })

                loadUserDetails();
            })
            .fail(function (response) {
                window.Global.HideLoadingSpinner();

                vm.errorHandling("Failed to add user credit", response);
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
            url: "/api/Admin/Users/" + vm.userId() + "/ResetPassword",
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

    function userClick(userId) {
        vm.userId(userId);

        MicroModal.show("user-details-modal");

        loadUserDetails();
    }

    function loadTasksList() {
        window.Global.ShowLoadingSpinner();

        $.ajax({
            url: "/api/Admin/TasksDropdown",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "GET",
        })
            .done(function (response) {
                window.Global.HideLoadingSpinner();

                vm.taskOptions(response.data)
            })
            .fail(function (response) {
                window.Global.HideLoadingSpinner();

                vm.errorHandling("Failed to load tasks", response);
            });
    }

    function loadUserDetails() {
        window.Global.ShowLoadingSpinner();

        $.ajax({
            url: "/api/Admin/Users/" + vm.userId(),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "GET",
        })
            .done(function (response) {
                window.Global.HideLoadingSpinner();

                setUserDetails(response.data);
            })
            .fail(function (response) {
                window.Global.HideLoadingSpinner();

                vm.errorHandling("Failed to load user details", response);
            });
    }

    function setUserDetails(responseData) {
        vm.viewType(1);
        vm.name(responseData.name);
        vm.loginUsername(responseData.loginUsername);
        vm.password(responseData.password);
        vm.currency(responseData.currency);
        vm.language(responseData.language);
        vm.walletAmount(responseData.walletAmount);
        vm.dateAdded(responseData.dateAdded);
        vm.accountStatus(responseData.accountStatus);

        vm.showPopupMessage(responseData.showPopupMessage);
        vm.popupMessageTitle(responseData.popupMessageTitle);
        vm.popupMessage(responseData.popupMessage);

        userTaskList = responseData.userTaskList;
        walletTransactionList = responseData.walletTransactionList;

        vm.viewTypeClick(vm.viewType());
    }

    function userTaskClick(status, userTaskId) {
        if (status == "Pending" || status == "Submitted") {
            var purchaseQuantity = 1;

            Swal.fire({
                icon: "info",
                title: "Approve User Task?",
                text: "By approving, wallet amount will be added",
                showConfirmButton: true,
                showCancelButton: true,
                showDenyButton: true,
                confirmButtonText: "Approve",
                denyButtonText: "Reject",
                input: "number",
                inputLabel: "Purchase Quantity",
                preConfirm: (quantity) => {
                    purchaseQuantity = quantity;
                }
            }).then((result) => {
                if (result.isConfirmed) {
                    updateUserTask(userTaskId, 2, purchaseQuantity);
                } else if (result.isDenied) {
                    updateUserTask(userTaskId, 3, null);
                }
            });
        } else if (status == "New Task") {
            Swal.fire({
                icon: "info",
                title: "Remove user task?",
                showConfirmButton: true,
                showCancelButton: true,
                confirmButtonText: "Remove",
            }).then((result) => {
                if (result.isConfirmed) {
                    updateUserTask(userTaskId, 4);
                }
            });
        }
    }

    function updateUserTask(userTaskId, newStatus, quantity) {
        window.Global.ShowLoadingSpinner();

        var data = {
            newStatus: newStatus,
            quantity: quantity
        };

        $.ajax({
            url: "/api/Admin/Users/" + vm.userId() + "/Usertasks/" + userTaskId + "/UpdateStatus",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "PUT",
            data: JSON.stringify(data)
        })
            .done(function (response) {
                window.Global.HideLoadingSpinner();

                Swal.fire({
                    icon: "success",
                    title: "Updated Task Status",
                    showConfirmButton: true,
                });

                loadUserDetails();
            })
            .fail(function (response) {
                window.Global.HideLoadingSpinner();

                vm.errorHandling("Failed to load users", response);
            });
    }

    function updateWalletTransactionClick(status, walletTransactionId) {
        if (status == "Pending") {
            Swal.fire({
                icon: "info",
                title: "Update Wallet Transaction Status",
                text: "Approve or Reject wallet transaction",
                showConfirmButton: true,
                showCancelButton: true,
                showDenyButton: true,
                confirmButtonText: "Approve",
                denyButtonText: "Reject"
            }).then((result) => {
                if (result.isConfirmed) {
                    updateWalletTransactionStatus(walletTransactionId, 1);
                } else if (result.isDenied) {
                    updateWalletTransactionStatus(walletTransactionId, 2);
                }
            });
        }
    }

    function updateWalletTransactionStatus(walletTransactionId, newStatus) {
        window.Global.ShowLoadingSpinner();

        var data = {
            newStatus: newStatus
        };

        $.ajax({
            url: "/api/Admin/Users/" + vm.userId() + "/Wallet/" + walletTransactionId + "/Update",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "PUT",
            data: JSON.stringify(data)
        })
            .done(function (response) {
                window.Global.HideLoadingSpinner();

                Swal.fire({
                    icon: "success",
                    title: "Updated Wallet Transaction Status",
                    showConfirmButton: true,
                });

                loadUserDetails();
            })
            .fail(function (response) {
                window.Global.HideLoadingSpinner();

                vm.errorHandling("Failed to load users", response);
            });
    }

    vm.updatePasswordClick = function () {
        if (vm.password() != vm.confirmPassword()) {
            Swal.fire({
                icon: "info",
                title: "The passwords needs to be identical to proceed",
            });

            return;
        }

        window.Global.ShowLoadingSpinner();

        var data = {
            password: vm.password()
        }

        $.ajax({
            url: "/api/Admin/UpdatePassword/",
            dataType: "json",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            type: "PUT",
        })
            .done(function (response) {
                window.Global.HideLoadingSpinner();

                Swal.fire({
                    icon: "success",
                    title: "Successfully updated password",
                });

                MicroModal.close("change-password-modal");

                vm.removeURLParam("rpc");
            })
            .fail(function (response) {
                window.Global.HideLoadingSpinner();

                vm.errorHandling("Failed to update password", response);
            });
    }

    window.pageViewModel = vm;

    return vm;
}