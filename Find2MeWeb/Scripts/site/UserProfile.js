var UserProfile = function (F2M_Utilities) {
    self = this;
    self._F2M_Utilities = F2M_Utilities;

    self.GetUserFollowersCount = function (userId) {
        return _F2M_Utilities.Ajax("/en/profile/getfollowers?UserId=" + userId, "GET", null)
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

};