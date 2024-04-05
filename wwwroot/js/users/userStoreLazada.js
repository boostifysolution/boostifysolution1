var userStoreLazada = new function () {
    var vm = window.pageViewModel;

    var userTaskId;
    var demo = false;

    vm.taskTitle = ko.observable();
    vm.productName = ko.observable();
    vm.productDescription = ko.observable();
    vm.productMainImageURL = ko.observable();
    vm.productImagesURL = ko.observableArray();
    vm.productPrice = ko.observable();
    vm.productRating = ko.observable();
    vm.productRatingCount = ko.observable();
    vm.productRatingWidth = ko.observable();
    vm.productSoldCount = ko.observable();
    vm.storeName = ko.observable();
    vm.storeThumbnailURL = ko.observable();
    vm.productReviewsList = ko.observableArray();
    vm.shippingFee = ko.observable();
    vm.vouchers = ko.observableArray();
    vm.quantityRemaining = ko.observable();
    vm.productQuantity = ko.observable(1);
    vm.active = ko.observable(false);
    vm.isDemo = ko.observable(false);
    vm.beforeDiscountPrice = ko.observable();
    vm.discountPercentage = ko.observable();

    var jsResx;

    vm.initialize = function (inJsResx) {
        jsResx = inJsResx;
        var url = new URL(window.location);
        userTaskId = url.searchParams.get("tId");
        demo = url.searchParams.get("demo");

        if (userTaskId != null) {
            if (demo != null && demo == "true") {
                demo = true;
                loadDemoStoreLazadaTaskDetails();
            } else {
                loadStoreLazadaTaskDetails();
            }
        } else {
            window.location.href = "/users/home";
        }
    }

    function loadDemoStoreLazadaTaskDetails() {
        window.Global.ShowLoadingSpinner();

        $.ajax({
            url: "/api/Admin/DemoTasks/" + userTaskId,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "GET",
        })
            .done(function (response) {
                window.Global.HideLoadingSpinner();

                setStoreLazadaTaskDetails(response.data);
            })
            .fail(function (response) {
                window.Global.HideLoadingSpinner();

                vm.errorHandling(jsResx.failedToLoadTask, response);
            });
    }

    function loadStoreLazadaTaskDetails() {
        window.Global.ShowLoadingSpinner();

        $.ajax({
            url: "/api/Users/" + vm.userId() + "/UserTasks/" + userTaskId,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "GET",
        })
            .done(function (response) {
                window.Global.HideLoadingSpinner();

                setStoreLazadaTaskDetails(response.data);
            })
            .fail(function (response) {
                window.Global.HideLoadingSpinner();

                vm.errorHandling(jsResx.failedToLoadTask, response);
            });
    }

    function setStoreLazadaTaskDetails(responseData) {
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
        vm.productSoldCount(responseData.productSoldCount);
        vm.productRatingCount(responseData.productRatingCount);
        vm.shippingFee(responseData.shippingFee);
        vm.vouchers(responseData.vouchers);
        vm.quantityRemaining(responseData.quantityRemaining);
        vm.isDemo(demo);
        vm.beforeDiscountPrice(responseData.beforeDiscountPrice);
        vm.discountPercentage(responseData.discountPercentage);
    }

    vm.productImageHover = function (data) {
        vm.productMainImageURL(data);
    }

    vm.minusQuantity = function () {
        if (vm.productQuantity() > 1) {
            var currentQuantity = vm.productQuantity();

            currentQuantity = currentQuantity - 1;

            vm.productQuantity(currentQuantity);
        }
    }

    vm.plusQuantity = function () {
        var currentQuantity = vm.productQuantity();

        currentQuantity = currentQuantity + 1;

        vm.productQuantity(currentQuantity);
    }

    vm.variationClick = function () {
        vm.active(!vm.active());
    }

    vm.addToCartUnavailable = function () {
        Swal.fire({
            icon: "info",
            title: jsResx.addToCartNotAvailable,
            text: jsResx.useBuyNow,
            showConfirmButton: true,
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

    vm.buyNowClick = function () {
        if (demo == true) {
            Swal.fire({
                icon: "info",
                title: "Demo Mode",
                text: "Buy Now Disabled",
                showCancelButton: true,
            })
        } else {
            Swal.fire({
                icon: "info",
                title: jsResx.buyNowConfirmation,
                text: jsResx.selectBuyNow,
                showConfirmButton: true,
                showCancelButton: true,
                confirmButtonText: "Buy Now"
            }).then((result) => {
                if (result.isConfirmed) {
                    buyNow(vm.productQuantity());
                }
            })
        }
    }


    vm.buyNowClickMobile = function () {
        if (demo == true) {
            Swal.fire({
                icon: "info",
                title: "Demo Mode",
                text: "Buy Now Disabled",
                showCancelButton: true,
            })
        } else {
            Swal.fire({
                icon: "info",
                input: "select",
                inputOptions: {
                    1: "1",
                    2: "2",
                    3: "3",
                    4: "4",
                    5: "5",
                    6: "6",
                    7: "7",
                    8: "8",
                    9: "9",
                    10: "10",
                },
                title: jsResx.buyNowConfirmation,
                text: jsResx.useBuyNowMobile,
                showConfirmButton: true,
                showCancelButton: true,
                confirmButtonText: "Buy Now"
            }).then((result) => {
                if (result.isConfirmed) {
                    buyNow(result.value);
                }
            })
        }
    }

    function buyNow(quantity) {
        window.Global.ShowLoadingSpinner();

        var data = {
            newStatus: 4,
            quantity: quantity,
        };

        $.ajax({
            url: "/api/Users/" + vm.userId() + "/UserTasks/" + userTaskId,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: 'PUT',
            data: JSON.stringify(data)
        }).done(function (response) {
            window.Global.HideLoadingSpinner();

            Swal.fire({
                icon: "success",
                title: jsResx.successfullyPurchaseProduct,
                text: jsResx.contactSupport,
                showConfirmButton: true,
            }).then(function (isConfirm) {
                window.location.href = "/users/home";
            })

            setTimeout(function () {
                window.location.href = "/users/home";
            }, 5000)

        }).fail(function (response) {
            window.Global.HideLoadingSpinner();

            vm.errorHandling(jsResx.failedBuyNow, response);
        });
    }



    window.pageViewModel = vm;

    return vm;
}