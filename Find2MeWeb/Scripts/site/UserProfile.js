var UserProfile = function (F2M_Utilities) {
    self = this;
    self._F2M_Utilities = F2M_Utilities;

    self.GetUserFollowersCount = function (userId) {
        return self._F2M_Utilities.Ajax("/en/profile/getfollowers?UserId=" + userId, "GET", null)
            .done(function (response) {
                if (response) {
                    if (response.Success) {
                        $('[data-control="follower-count"]').empty().text(response.Data.Followers);
                        $('[data-control="followed-count"]').empty().text(response.Data.Followed);
                    }
                }
            })
            .fail(function (response) {

            });
    };


    /*
    data: {
        __RequestVerificationToken: token,
        id: username
    }
    */
    self.ValidateUsername = function (data) {
        self._F2M_Utilities.AjaxNoContentType("/en/Account/UserExists", "POST", data, "")
            .done(function (data) {
                //enable form Submit Buttons
                $('input[name="SubmitAction"]').removeAttr("disabled");

                if (data) {
                    if (data.UserExists) {
                        var suggestionsHtml = "<strong>Suggestions: </strong>";
                        $.each(data.UserSuggestion, function (index, item) {
                            suggestionsHtml += '<span><a href="javascript:void(0)" data-value="' + item + '">' + item + '</a>&nbsp;&nbsp;&nbsp;</span>';
                        });

                        $('#usernames_suggestion_wrapper').empty().html(suggestionsHtml);
                        $('#valid_username_icon').hide();
                        $('#invalid_username_icon').show();
                        //disable form Submit Buttons
                        $('input[name="SubmitAction"][value="Next"]').attr("disabled", "disabled");
                    }
                    else {
                        $('#valid_username_icon').show();
                        $('#invalid_username_icon').hide();
                        //enable form Submit Buttons
                        $('input[name="SubmitAction"]').removeAttr("disabled");
                    }
                }
            })
            .fail(function (data) {
                //enable form Submit Buttons
                $('input[name="SubmitAction"]').removeAttr("disabled");
            });
    };

    self.SendEmailConfirmationLink = function (email) {
        return self._F2M_Utilities.Ajax("/en/account/sendconfirmemail?email=" + email, "GET", null)
            .done(function (data) {
                //enable form Submit Buttons
                $('#send_email_confirm_btn').removeAttr("disabled");
                $('#send_email_confirm_btn').val("Resend Confirmation Link");
                if (data) {
                    if (data.Success) {
                        $('[data-email-confirmation-alert=""]').show();
                    }
                }
                
            })
            .fail(function (data) {
                $('[data-email-confirmation-alert=""]').hide();
            });
    };
};