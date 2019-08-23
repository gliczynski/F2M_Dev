namespace Find2Me.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userAds_tbl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserAdImages",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AdID = c.Int(nullable: false),
                        AdImageOriginal = c.String(),
                        AdImageSelected = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserAds", t => t.AdID, cascadeDelete: true)
                .Index(t => t.AdID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserAdImages", "AdID", "dbo.UserAds");
            DropIndex("dbo.UserAdImages", new[] { "AdID" });
            DropTable("dbo.UserAdImages");
        }
    }
}
