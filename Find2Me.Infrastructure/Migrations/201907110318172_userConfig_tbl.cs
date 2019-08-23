namespace Find2Me.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userConfig_tbl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserConfigurations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.String(maxLength: 128),
                        ConfiguratonID = c.Int(nullable: false),
                        Value = c.String(),
                        CreatedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .ForeignKey("dbo.Configurations", t => t.ConfiguratonID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.ConfiguratonID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserConfigurations", "ConfiguratonID", "dbo.Configurations");
            DropForeignKey("dbo.UserConfigurations", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.UserConfigurations", new[] { "ConfiguratonID" });
            DropIndex("dbo.UserConfigurations", new[] { "UserID" });
            DropTable("dbo.UserConfigurations");
        }
    }
}
