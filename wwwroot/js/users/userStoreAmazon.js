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
    vm.comments = ko.observableArray();

    vm.initialize = function () {
        loadStoreAmazonTaskDetails();
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
        vm.comments(responseData.comments);
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



    window.pageViewModel = vm;

    return vm;
}