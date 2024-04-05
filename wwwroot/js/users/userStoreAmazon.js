var userStoreAmazon = new function () {
    var vm = window.pageViewModel;

    var taskId;

    vm.taskTitle = ko.observable();
    vm.productName = ko.observable();
    vm.productDescription = ko.observable();
    vm.productMainImageURL = ko.observable();
    vm.productImagesURL = ko.observable();
    vm.productPrice = ko.observable();
    vm.productRating = ko.observable();
    vm.storeName = ko.observable();
    vm.storeThumbnailURL = ko.observable();
    vm.productReviewsList = ko.observableArray();
    vm.supportItemsList = ko.observableArray();
    vm.productRatingWidth = ko.observable();
    vm.ratingCount = ko.observable();
    vm.deliveryDate = ko.observable();
    vm.quantity = ko.observable();
    vm.quantityOptions = ko.observableArray([{ id: 1, text: 1}, { id: 2, text: 2},{ id: 3, text: 3},{ id: 4, text: 4},{ id: 5, text: 5},{ id: 6, text: 6},{ id: 7, text: 7},{ id: 8, text: 8},{ id: 9, text: 9},{ id: 10, text: 10} ])
    

    vm.initialize = function () {
        var url = new URL(window.location);
        userTaskId = url.searchParams.get("tId");
        demo = url.searchParams.get("demo");

        if (userTaskId != null) {
            if (demo != null && demo == "true") {
                demo = true;
                loadDemoStoreAmazonTaskDetails();
            } else {
                loadStoreAmazonTaskDetails();
            }
        } else {
            window.location.href = "/users/home";
        }
    }

    function loadDemoStoreAmazonTaskDetails() {
        window.Global.ShowLoadingSpinner();

        $.ajax({
            url: "/api/Admin/DemoTasks/" + userTaskId,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "GET",
        })
            .done(function (response) {
                window.Global.HideLoadingSpinner();

                setStoreAmazonTaskDetails(response.data);
            })
            .fail(function (response) {
                window.Global.HideLoadingSpinner();

                vm.errorHandling(jsResx.failedToLoadTask, response);
            });
    }

    function loadStoreAmazonTaskDetails() {
        window.Global.ShowLoadingSpinner();

        $.ajax({
            url: "/api/Users/" + vm.userId() + "/Tasks/" + taskId,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "GET",
        })
            .done(function (response) {
                window.Global.HideLoadingSpinner();

                setStoreAmazonTaskDetails(response.data);
            })
            .fail(function (response) {
                window.Global.HideLoadingSpinner();

                vm.errorHandling("Failed to load store task details", response);
            });
    }

    function setStoreAmazonTaskDetails(responseData) {
        vm.taskTitle(responseData.taskTitle);
        vm.productName(responseData.productName);
        vm.productDescription(responseData.productDescription);
        vm.productMainImageURL(responseData.productMainImageURL);
        vm.productImagesURL(responseData.productImagesURL);
        vm.productPrice(responseData.productPrice);
        vm.productRating(responseData.productRating);
        vm.storeName(responseData.storeName);
        vm.storeThumbnailURL(responseData.storeThumbnailURL);
        vm.productReviewsList(responseData.productReviewsList);
        vm.supportItemsList(responseData.supportItemsList);
        vm.productRatingWidth(responseData.productRatingWidth);
        vm.ratingCount(responseData.productRatingCount);
        moment.locale('ja');
        vm.deliveryDate(moment().add(4, 'days').format('M月D日'));
    }

    vm.updateStoreAmazonTaskClick = function () {
        window.Global.ShowLoadingSpinner();

        $.ajax({
            url: "/api/Users/" + vm.userId() + "/Tasks/" + taskId,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: 'PUT',
        }).done(function (response) {
            window.Global.HideLoadingSpinner();

            Swal.fire({
                icon: "success",
                title: "Task Successfully Completed",
                showConfirmButton: true,
            }).then(function (isConfirm) {
                window.location.href = "/users/home";
            })

            setTimeout(function () {
                window.location.href = "/users/home";
            }, 3000)

        }).fail(function (response) {
            window.Global.HideLoadingSpinner();

            vm.errorHandling("Failed to save user profile", response);
        });
    }

    vm.disabledNavigation = function () {
        Swal.fire({
            icon: "info",
            title: jsResx.disabledNavigation,
            text: jsResx.switchPage,
            showConfirmButton: true,
        });
    }

    vm.productImageHover = function (data) {
        vm.productMainImageURL(data);
    }



    window.pageViewModel = vm;

    return vm;
}