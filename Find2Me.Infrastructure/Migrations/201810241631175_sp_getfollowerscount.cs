namespace Find2Me.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class sp_getfollowerscount : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
                name: "dbo.SP_GetFollowersCount",
                parametersAction: p => new
                {
                    UserId = p.String(maxLength: 128),
                },
                body: @"SELECT 
	                    COALESCE(SUM(CASE FollowedUserId WHEN @UserId THEN 1 ELSE 0 END), 0) AS Followers, 
	                    COALESCE(SUM(CASE FollowByUserId WHEN @UserId THEN 1 ELSE 0 END), 0) AS Followed
	                    FROM UserFollowers"
                );
        }

        public override void Down()
        {
            DropStoredProcedure(name: "dbo.SP_GetFollowersCount");
        }
    }
}
