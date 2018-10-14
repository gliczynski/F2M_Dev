

class Find2MeScripts {

    ShowPageAlert(Control, Success, Message) {
        if (Success) {
            if (Success === true) {
                Control.show().removeClass('alert-warning').addClass('alert-success').empty().html('<i class="fa fa-check-circle fa-fw"></i> ' + Message);
            }
            else {
                Control.show().removeClass('alert-success').addClass('alert-warning').empty().html('<i class="fa fa-exclamation-circle fa-fw"></i>' + Message);
            }
        }
    }

}

