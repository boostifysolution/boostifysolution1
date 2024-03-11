var adminSignUp = new function () {
    var vm = this;

    vm.name = ko.observable();
    vm.email = ko.observable();
    vm.phoneNumber = ko.observable();
    vm.country = ko.observable();
    vm.countryOptions = ko.observableArray([
        {
            text: "Malaysia",
            id: 0
        },
        {
            text: "Indonesia",
            id: 1
        },
        {
            text: "India",
            id: 2
        }
    ]);

    vm.signUpClick = function () {

    }



    window.pageViewModel = vm;

    return vm;
}