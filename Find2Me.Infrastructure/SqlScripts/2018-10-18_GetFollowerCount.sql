CREATE PROCEDURE [dbo].[SP_GetFollowersCount]
    @UserId [nvarchar](128)
AS
BEGIN
    SELECT 
    	                    SUM(CASE FollowedUserId WHEN @UserId THEN 1 ELSE 0 END) AS Followers, 
    	                    SUM(CASE FollowByUserId WHEN @UserId THEN 1 ELSE 0 END) AS Followed
    	                    FROM UserFollowers
END