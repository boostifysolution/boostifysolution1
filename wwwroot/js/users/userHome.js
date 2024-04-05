var userHome = new function () {
    var vm = window.pageViewModel;

    vm.userTasks = ko.observableArray();
    vm.userSubmittedTasks = ko.observableArray();
    vm.completedUserTasks = ko.observableArray();
    vm.walletAmount = ko.observable();
    vm.walletLastUpdated = ko.observable();
    vm.userName = ko.observable();
    vm.accountStatus = ko.observable();
    vm.accountStatusMessage = ko.observable();

    vm.initialize = function () {
        loadHomeDetails();
    }

    function loadHomeDetails() {
        window.Global.ShowLoadingSpinner();

        $.ajax({
            url: "/api/Users/" + vm.userId() + "/Home",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "GET",
        })
            .done(function (response) {
                window.Global.HideLoadingSpinner();

                setHomeDetails(response.data);
            })
            .fail(function (response) {
                window.Global.HideLoadingSpinner();

                vm.errorHandling("Failed to load home details", response);
            });
    }

    vm.startTaskClick = function (data) {
        var taskURL = "/users/"
        switch (data.platform) {
            case 0:
                taskURL = taskURL + "shopee?";
                break;
            case 1:
                taskURL = taskURL + "lazada?";
                break;
            case 2:
                taskURL = taskURL + "amazon?";
                break;
        }

        taskURL = taskURL + "tId=" + data.userTaskId;

        switch (data.language) {
            case 0:
                taskURL = taskURL + "&culture=en-gb";
                break;
            case 1:
                taskURL = taskURL + "&culture=ms-my";
                break;
            case 2:
                taskURL = taskURL + "&culture=zh";
                break;
            case 3:
                taskURL = taskURL + "&culture=ja-JP";
                break;
        }

        window.location.href = taskURL;
    }

    function setHomeDetails(responseData) {
        vm.userTasks(responseData.userTasks);
        vm.completedUserTasks(responseData.completedUserTasks);
        vm.userSubmittedTasks(responseData.userSubmittedTasks);
        vm.walletAmount(responseData.walletAmount);
        vm.userName(responseData.userName);
        vm.accountStatus(responseData.accountStatus);
        vm.accountStatusMessage(responseData.accountStatusMessage);
        vm.walletLastUpdated(responseData.walletLastUpdated);
    }

    window.pageViewModel = vm;

    return vm;
}