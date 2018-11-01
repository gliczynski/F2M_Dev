namespace Find2Me.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class sp_delete_userprofile_data : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
                name: "dbo.SP_DeleteUserProfileData",
                parametersAction: p => new
                {
                    UserId = p.String(maxLength: 128),
                },
                body: @"
DELETE FROM AspNetUserLogins WHERE UserId = @UserId
DELETE FROM AspNetUserClaims WHERE UserId = @UserId
DELETE FROM UserFollowers WHERE FollowByUserId = @UserId OR FollowedUserId = @UserId"
                );
        }

        public override void Down()
        {
            DropStoredProcedure(name: "dbo.SP_DeleteUserProfileData");
        }
    }
}
