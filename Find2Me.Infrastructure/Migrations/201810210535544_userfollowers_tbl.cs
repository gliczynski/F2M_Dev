namespace Find2Me.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userfollowers_tbl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ActionType = c.String(),
                        ActionMessage = c.String(),
                        IpAddress = c.String(),
                        CreateTime = c.DateTime(nullable: false),
                        ActionFromUserId = c.String(maxLength: 128),
                        ActionToUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ActionFromUserId)
                .ForeignKey("dbo.AspNetUsers", t => t.ActionToUserId)
                .Index(t => t.ActionFromUserId)
                .Index(t => t.ActionToUserId);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.FollowByUserId)
                .ForeignKey("dbo.AspNetUsers", t => t.FollowedUserId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id1)
                .Index(t => t.FollowByUserId)
                .Index(t => t.FollowedUserId)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id1);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Logs", "ActionToUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Logs", "ActionFromUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserFollowers", "ApplicationUser_Id1", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserFollowers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserFollowers", "FollowedUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserFollowers", "FollowByUserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserFollowers", new[] { "ApplicationUser_Id1" });
            DropIndex("dbo.UserFollowers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.UserFollowers", new[] { "FollowedUserId" });
            DropIndex("dbo.UserFollowers", new[] { "FollowByUserId" });
            DropIndex("dbo.Logs", new[] { "ActionToUserId" });
            DropIndex("dbo.Logs", new[] { "ActionFromUserId" });
            DropTable("dbo.UserFollowers");
            DropTable("dbo.Logs");
        }
    }
}
