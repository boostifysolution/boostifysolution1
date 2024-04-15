window.Global = {
    ShowLoadingSpinner: function () {
        $('#loader').show();
    },
    HideLoadingSpinner: function () {
        $('#loader').hide();
    },
    RemoveURLParam: function (url, key) {

        var rtn = url.split("?")[0],
            param,
            params_arr = [],
            queryString = (url.indexOf("?") !== -1) ? url.split("?")[1] : "";
        if (queryString !== "") {
            params_arr = queryString.split("&");
            for (var i = params_arr.length - 1; i >= 0; i -= 1) {
                param = params_arr[i].split("=")[0];
                if (param === key) {
                    params_arr.splice(i, 1);
                }
            }
            rtn = rtn + "?" + params_arr.join("&");
        }
        return rtn;
    },
    UpdateURLParamValue: function (url, paramKey, paramVal) {
        var newAdditionalURL = "";
        var tempArray = url.split("?");
        var baseURL = tempArray[0];
        var additionalURL = tempArray[1];
        var temp = "";
        if (additionalURL) {
            tempArray = additionalURL.split("&");
            for (var i = 0; i < tempArray.length; i++) {
                if (tempArray[i].split('=')[0] != paramKey) {
                    newAdditionalURL += temp + tempArray[i];
                    temp = "&";
                }
            }
        }

        var rows_txt = "";

        if (paramVal != undefined && paramVal != null) {
            rows_txt = temp + "" + paramKey + "=" + paramVal;
        }
        return baseURL + "?" + newAdditionalURL + rows_txt;
    },
    GetIDFromURL: function (url, position) {
        var splitURL = url.split('/');
        return splitURL[splitURL.length - position];
    }
};

// example: "dog".pluralize(4, "dogs")
String.prototype.pluralize = function (count, plural) {
    return (count == 1 ? this : plural);
}

$(document).ready(function () {
    ko.validation.init({
        registerExtenders: true,
        messagesOnModified: true,
        insertMessages: true,
        parseInputAttributes: true,
        messageTemplate: null,
        errorElementClass: 'has-error',
    }, true);

    ko.bindingHandlers.onEnter = {
        init: function (element, valueAccessor, allBindings, data, context) {
            var wrapper = function (data, event) {
                if (event.keyCode === 13) {
                    valueAccessor().call(this, data, event);
                }
            };
            ko.applyBindingsToNode(element, { event: { keyup: wrapper } }, context);
        }
    };

    if (typeof window.pageViewModel !== "undefined") {
        ko.applyBindings(window.pageViewModel);
    }
    else {
        ko.applyBindings({});
    }

    $("a[rel~='keep-params']").click(function(e) {
        e.preventDefault();
    
        var params = window.location.search,
            dest = $(this).attr('href') + params;
    
        // in my experience, a short timeout has helped overcome browser bugs
        window.setTimeout(function() {
            window.location.href = dest;
        }, 100);
    });
});