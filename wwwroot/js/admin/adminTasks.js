

var adminTasks = new function () {
    var vm = window.pageViewModel;

    vm.taskId = ko.observable();
    vm.taskTitle = ko.observable();
    vm.productName = ko.observable();
    vm.productDescription = ko.observable();
    vm.productMainImageURL = ko.observable();
    vm.productImagesURL = ko.observableArray();
    vm.productImagesArray = ko.observableArray();
    vm.productPrice = ko.observable();
    vm.productRating = ko.observable();
    vm.storeName = ko.observable();
    vm.storeThumbnailURL = ko.observable();
    vm.status = ko.observable();
    vm.language = ko.observable();
    vm.languageOptions = ko.observableArray();
    vm.platform = ko.observable();
    vm.platformOptions = ko.observableArray();
    vm.country = ko.observableArray();
    vm.countryOptions = ko.observableArray();
    vm.taskCategory = ko.observable();
    vm.taskCategoryOptions = ko.observableArray();

    //filters
    vm.filterTaskTitle = ko.observable();
    vm.filterProductName = ko.observable();
    vm.filterTaskCategory = ko.observable();
    vm.filterCountry = ko.observable();
    vm.filterLanguage = ko.observable();
    vm.filterTaskStatus = ko.observable();
    vm.taskStatusOptions = ko.observable();

    vm.dateAdded = ko.observable();

    var productDescriptionEditor;
    var dataFilters;

    vm.productImagesURL.subscribe(function (input) {
        vm.productImagesArray([]);
        if (input != null) {
            if (input.includes(",")) {
                var splitImages = input.split(",");

                splitImages.forEach(function (x) {
                    vm.productImagesArray.push(x.trim());
                });
            } else {
                vm.productImagesArray.push(input);
            }
        }

    })

    vm.initialize = function () {
        productDescriptionEditor = ClassicEditor
            .create(document.querySelector('#productDescriptionEditor')
            )
            .then(newEditor => {
                productDescriptionEditor = newEditor;
            })
            .catch(error => {
                console.error(error);
            });

        loadTasks();
    }

    vm.applyFilters = function () {
        dataFilters = {
            filterTaskTitle: vm.filterTaskTitle(),
            filterProductName: vm.filterProductName(),
            filterTaskCategory: vm.filterTaskCategory(),
            filterCountry: vm.filterCountry(),
            filterLanguage: vm.filterLanguage(),
            filterTaskStatus: vm.filterTaskStatus()
        };

        $("#tasksGrid").jsGrid("loadData");
    }

    vm.clearFilters = function () {
        vm.filterTaskTitle(null);
        vm.filterProductName(null);
        vm.filterTaskCategory(null);
        vm.filterCountry(null);
        vm.filterLanguage(null);
        vm.filterTaskStatus(null);

        vm.applyFilters();
    }

    vm.addNewTaskClick = function () {
        vm.taskId(0);
        vm.taskTitle(null);
        vm.productName(null);
        vm.productDescription(null);
        vm.productMainImageURL(null);
        vm.productImagesURL(null);
        vm.productPrice(null);
        vm.productRating(null);
        vm.storeName(null);
        vm.storeThumbnailURL(null);
        vm.language(null);
        vm.taskCategory(null);

        productDescriptionEditor.setData("");

        MicroModal.show("task-details-modal");
    }

    vm.saveTaskDetails = function () {
        window.Global.ShowLoadingSpinner();

        var data = {
            taskTitle: vm.taskTitle(),
            productName: vm.productName(),
            productMainImageURL: vm.productMainImageURL(),
            productImagesURL: vm.productImagesURL(),
            productPrice: vm.productPrice(),
            productRating: vm.productRating(),
            storeName: vm.storeName(),
            storeThumbnailURL: vm.storeThumbnailURL(),
            language: vm.language(),
            productDescription: productDescriptionEditor.getData(),
            taskCategory: vm.taskCategory(),
            platform: vm.platform(),
            country: vm.country()
        }

        $.ajax({
            url: "/api/Admin/Tasks/" + vm.taskId(),
            dataType: "json",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            type: "PUT",
        })
            .done(function (response) {
                Swal.fire({
                    icon: "success",
                    title: "Task Successfully Saved",
                    showConfirmButton: true,
                }).then((result) => {
                    MicroModal.close("task-details-modal");
                })

                $("#tasksGrid").jsGrid("loadData");
            })
            .fail(function (response) {
                window.Global.HideLoadingSpinner();

                vm.errorHandling("Failed to create new users", response);
            });
    }

    function loadTasks() {
        window.Global.ShowLoadingSpinner();

        $("#tasksGrid").jsGrid({
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
            noDataContent: "No task yet",
            rowClick: function (args) {
                taskClick(args.item.taskId);
            },
            fields: [
                {
                    name: "taskTitle",
                    type: "text",
                    title: "Task Title",
                    width: 150
                },
                {
                    name: "productName",
                    type: "text",
                    title: "Product Name",
                    width: 150
                },
                {
                    name: "category",
                    type: "text",
                    title: "Category",
                    width: 100
                },
                {
                    name: "country",
                    type: "text",
                    title: "Country",
                    width: 80
                },
                {
                    name: "language",
                    type: "text",
                    title: "Language",
                    width: 80
                },
                {
                    name: "productPrice",
                    type: "text",
                    title: "Product Price",
                    width: 100
                },
                {
                    name: "status",
                    type: "text",
                    title: "Status",
                    width: 80,
                },
                {
                    name: "dateAdded",
                    type: "text",
                    title: "Date Added",
                    width: 80,
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
                        url: "/api/Admin/Tasks",
                        dataType: "json",
                        data: filters,
                        contentType: "application/json; charset=utf-8",
                        type: "GET",
                    })
                        .done(function (response) {
                            var da = {
                                data: response.data.tasksList,
                                itemsCount: response.data.itemsCount,
                            };

                            vm.taskCategoryOptions(response.data.taskCategoryOptions);
                            vm.platformOptions(response.data.platformOptions);
                            vm.countryOptions(response.data.countryOptions);
                            vm.languageOptions(response.data.languageOptions);
                            vm.taskStatusOptions(response.data.taskStatusOptions);

                            d.resolve(da);
                            window.Global.HideLoadingSpinner();
                        })
                        .fail(function (response) {
                            window.Global.HideLoadingSpinner();

                            vm.errorHandling("Failed to load tasks", response);
                        });

                    return d.promise();
                },
            },
        });
    }

    function taskClick(taskId) {
        vm.taskId(taskId);

        MicroModal.show("task-details-modal");

        loadTaskDetails();
    }

    function loadTaskDetails() {
        window.Global.ShowLoadingSpinner();

        $.ajax({
            url: "/api/Admin/Tasks/" + vm.taskId(),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "GET",
        })
            .done(function (response) {
                window.Global.HideLoadingSpinner();

                setTaskDetails(response.data);
            })
            .fail(function (response) {
                window.Global.HideLoadingSpinner();

                vm.errorHandling("Failed to load task details", response);
            });
    }

    function setTaskDetails(responseData) {
        vm.taskTitle(responseData.taskTitle);
        vm.productName(responseData.productName);
        vm.productDescription(responseData.productDescription);
        vm.productMainImageURL(responseData.productMainImageURL);
        vm.productImagesURL(responseData.productImagesURL);
        vm.productPrice(responseData.productPrice);
        vm.productRating(responseData.productRating);
        vm.storeName(responseData.storeName);
        vm.storeThumbnailURL(responseData.storeThumbnailURL);
        vm.status(responseData.status);
        vm.language(responseData.language);
        vm.platform(responseData.platform);
        vm.taskCategory(responseData.taskCategory);
        vm.country(responseData.country);

        productDescriptionEditor.setData(responseData.productDescription);
    }

    vm.viewStoreClick = function () {
        if (vm.platform() == 0) {
            window.open("/users/shopee?tId=" + vm.taskId() + "&demo=true", "_blank");
        }else if(vm.platform() == 1){
            window.open("/users/lazada?tId=" + vm.taskId() + "&demo=true", "_blank");
        }else{
            window.open("/users/amazon?tId=" + vm.taskId() + "&demo=true&culture=ja-JP", "_blank");
        }
    }

    window.pageViewModel = vm;

    return vm;
}