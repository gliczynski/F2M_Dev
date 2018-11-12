
var F2M_Utilities = function () {
    self = this;

    self.Disable = function (control) {
        $(control).attr("disabled", "disabled");
    };
    self.Enable = function (control) {
        $(control).removeAttr("disabled");
    };

    self.Ajax = function (url, method, data, contentType = "application/json") {
        return $.ajax(url, {
            method: method,
            data: data,
            dataType: "json",
            contentType: contentType,
            //contentType: false,
            success: function (response) {
                return response;
            }
        });
    };

    self.AjaxNoContentType = function (url, method, data) {
        return $.ajax(url, {
            method: method,
            data: data,
            dataType: "json",
            //contentType: false,
            success: function (response) {
                return response;
            }
        });
    };

    self.ShowPageAlert = function (Control, Success, Message) {
        if (Success === true) {
            Control.slideDown().removeClass('alert-warning').addClass('alert-success').empty().html('<i class="fa fa-check-circle fa-fw"></i> ' + Message);
        }
        else {
            Control.slideDown().removeClass('alert-success').addClass('alert-warning').empty().html('<i class="fa fa-exclamation-circle fa-fw"></i>' + Message);
        }
    };
};