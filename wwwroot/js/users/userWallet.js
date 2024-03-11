var userWallet = new function () {
    var vm = window.pageViewModel;

    vm.walletAmount = ko.observable();
    vm.walletAmountDecimal = ko.observable();
    vm.walletLastUpdated = ko.observable();
    vm.walletTransactions = ko.observableArray();
    vm.bankName = ko.observable();
    vm.bankAccountNumber = ko.observable();
    vm.accountHolderName = ko.observable();
    vm.bankName = ko.observable();
    vm.isfcCode = ko.observable();
    vm.withdrawAmount = ko.observable();
    vm.showPopupMessage = ko.observable();
    vm.popupTitle = ko.observable();
    vm.popupMessage = ko.observable();

    vm.depositAmount = ko.observable();
    vm.depositMethodTypes = ko.observableArray([{ id: 0, text: "Debit/Credit Card" }, { id: 1, text: "Bank Transfer" }, { id: 2, text: "FPX" }, { id: 3, text: "Western Union" }, { id: 4, text: "Union Pay" },]);
    vm.depositMethod = ko.observable();

    var popupMessage;
    var popupMessageTitle;
    var showPopupMessage;


    vm.initialize = function () {
        loadWalletTransactions();
    }

    function loadWalletTransactions() {
        window.Global.ShowLoadingSpinner();

        $.ajax({
            url: "/api/Users/" + vm.userId() + "/Wallet",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "GET",
        })
            .done(function (response) {
                window.Global.HideLoadingSpinner();

                setWalletTransactions(response.data);
            })
            .fail(function (response) {
                window.Global.HideLoadingSpinner();

                vm.errorHandling("Failed to load wallet details", response);
            });
    }

    function setWalletTransactions(responseData) {
        vm.walletAmount(responseData.walletAmount);
        vm.walletLastUpdated(responseData.walletLastUpdated);
        vm.walletTransactions(responseData.walletTransactions);
        vm.walletAmountDecimal(responseData.walletAmountDecimal);

        popupMessage = responseData.popupMessage;
        popupMessageTitle = responseData.popupMessageTitle;
        showPopupMessage = responseData.showPopupMessage;

        if (showPopupMessage) {
            displayPopupMessage();
        }
    }

    vm.withdrawClick = function () {
        if (showPopupMessage) {
            displayPopupMessage();
            return;
        }

        vm.bankName(null);
        vm.bankAccountNumber(null);
        vm.accountHolderName(null);
        vm.isfcCode(null);
        vm.withdrawAmount(null);

        MicroModal.show("wallet-withdraw-modal");

    }

    vm.submitWithdrawClick = function (data) {
        if (showPopupMessage) {
            displayPopupMessage();
            return;
        }

        if (vm.bankAccountNumber() == null || vm.bankName() == null || vm.accountHolderName() == null || vm.withdrawAmount() == null) {
            Swal.fire({
                icon: 'info',
                title: 'Please complete required fields',
                text: 'Make sure all fields are filled before submitting',
                showConfirmButton: true
            });

            return;
        }

        if (vm.withdrawAmount() > vm.walletAmountDecimal()) {
            Swal.fire({
                icon: 'info',
                title: 'Insuffient Amount to Withdraw',
                text: 'You can only withdraw up to ' + vm.walletAmount(),
                showConfirmButton: true
            });

            return;
        }

        window.Global.ShowLoadingSpinner();

        var data = {
            bankAccountNumber: vm.bankAccountNumber(),
            accountHolderName: vm.accountHolderName(),
            bankName: vm.bankName(),
            isfcCode: vm.isfcCode(),
            withdrawAmount: vm.withdrawAmount(),
        }

        $.ajax({
            url: "/api/Users/" + vm.userId() + "/Wallet/Withdraw",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "POST",
            data: JSON.stringify(data)
        })
            .done(function (response) {
                window.Global.HideLoadingSpinner();

                Swal.fire({
                    icon: 'success',
                    title: 'Wallet Withdraw Submitted',
                    showConfirmButton: true
                });

                vm.walletTransactions.push(response.data);

                MicroModal.close("wallet-withdraw-modal");
            })
            .fail(function (response) {
                window.Global.HideLoadingSpinner();

                vm.errorHandling("Failed to load withdraw wallet", response);
            });
    }

    vm.walletTransactionClick = function (data) {
        if (showPopupMessage) {
            displayPopupMessage();
            return;
        }

        if (data.status == "Pending") {
            Swal.fire({
                icon: 'info',
                title: "Please contact your support admin",
                text: "Unable to get latest info about withdraw",
                showConfirmButton: true
            });
        }
    }

    vm.depositClick = function () {
        if (showPopupMessage) {
            displayPopupMessage();
            return;
        }

        vm.depositMethod(null);
        vm.depositAmount(null);

        MicroModal.show("wallet-deposit-modal");
    }

    vm.submitDepositClick = function () {
        if (showPopupMessage) {
            displayPopupMessage();
            return;
        }

        window.Global.ShowLoadingSpinner();

        var data = {
            depositMethod: vm.depositMethod(),
            depositAmount: vm.depositAmount()
        }

        $.ajax({
            url: "/api/Users/" + vm.userId() + "/Wallet/Deposit",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "POST",
            data: JSON.stringify(data)
        })
            .done(function (response) {
                window.Global.HideLoadingSpinner();

                Swal.fire({
                    icon: 'info',
                    title: response.data,
                    showConfirmButton: true
                });
            })
            .fail(function (response) {
                window.Global.HideLoadingSpinner();

                vm.errorHandling("Failed to load deposit wallet", response);
            });
    }

    function displayPopupMessage() {
        Swal.fire({
            icon: "error",
            title: popupMessageTitle,
            text: popupMessage,
            showConfirmButton: false,
            showCancelButton: false,
            allowOutsideClick: false,
            allowEscapeKey: false,
        });
    }

    window.pageViewModel = vm;

    return vm;
}