namespace Find2Me.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userfollowers_tbl_remove : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserFollowers", "FollowByUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserFollowers", "FollowedUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserFollowers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserFollowers", "ApplicationUser_Id1", "dbo.AspNetUsers");
            DropIndex("dbo.UserFollowers", new[] { "FollowByUserId" });
            DropIndex("dbo.UserFollowers", new[] { "FollowedUserId" });
            DropIndex("dbo.UserFollowers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.UserFollowers", new[] { "ApplicationUser_Id1" });
            DropTable("dbo.UserFollowers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserFollowers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FollowByUserId = c.String(maxLength: 128),
                        FollowedUserId = c.String(maxLength: 128),
                        FollowedOn = c.DateTime(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        ApplicationUser_Id1 = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.UserFollowers", "ApplicationUser_Id1");
            CreateIndex("dbo.UserFollowers", "ApplicationUser_Id");
            CreateIndex("dbo.UserFollowers", "FollowedUserId");
            CreateIndex("dbo.UserFollowers", "FollowByUserId");
            AddForeignKey("dbo.UserFollowers", "ApplicationUser_Id1", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.UserFollowers", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.UserFollowers", "FollowedUserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.UserFollowers", "FollowByUserId", "dbo.AspNetUsers", "Id");
        }
    }
}
