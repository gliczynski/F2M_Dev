namespace Find2Me.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dbupadate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserAdPriceRewards",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AdID = c.Int(nullable: false),
                        PriceCategoryId = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        IsFree = c.Boolean(nullable: false),
                        PriceMin = c.Double(nullable: false),
                        PriceMax = c.Double(nullable: false),
                        AdStartDate = c.DateTime(nullable: false),
                        AdEndDate = c.DateTime(nullable: false),
                        AdDuration = c.Int(nullable: false),
                        IsReward = c.Boolean(nullable: false),
                        RewardAmount = c.Double(nullable: false),
                        RewardExpiration = c.Double(nullable: false),
                        IsExclusiveAd = c.Boolean(nullable: false),
                        ExclusiveAdPeriod = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserAds", t => t.AdID, cascadeDelete: true)
                .Index(t => t.AdID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserAdPriceRewards", "AdID", "dbo.UserAds");
            DropIndex("dbo.UserAdPriceRewards", new[] { "AdID" });
            DropTable("dbo.UserAdPriceRewards");
        }
    }
}
