namespace Find2Me.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userfollowers_tbl_add : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserFollowers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FollowByUserId = c.String(maxLength: 128),
                        FollowedUserId = c.String(maxLength: 128),
                        FollowedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.FollowByUserId)
                .ForeignKey("dbo.AspNetUsers", t => t.FollowedUserId)
                .Index(t => t.FollowByUserId)
                .Index(t => t.FollowedUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserFollowers", "FollowedUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserFollowers", "FollowByUserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserFollowers", new[] { "FollowedUserId" });
            DropIndex("dbo.UserFollowers", new[] { "FollowByUserId" });
            DropTable("dbo.UserFollowers");
        }
    }
}
