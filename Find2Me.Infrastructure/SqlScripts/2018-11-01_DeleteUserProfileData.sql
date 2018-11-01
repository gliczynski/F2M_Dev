CREATE PROCEDURE SP_DeleteProfileData
    @UserId [nvarchar](128)
AS
BEGIN
	DELETE FROM AspNetUserLogins WHERE UserId = @UserId
	DELETE FROM AspNetUserClaims WHERE UserId = @UserId
    DELETE FROM UserFollowers WHERE FollowByUserId = @UserId OR FollowedUserId = @UserId
END