namespace Find2Me.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class useradinformation_tbl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserAdInformations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AdID = c.Int(nullable: false),
                        Category = c.String(),
                        IsOriginal = c.Boolean(nullable: false),
                        IsState = c.Boolean(nullable: false),
                        Headline = c.String(),
                        Description = c.String(),
                        Tags = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserAds", t => t.AdID, cascadeDelete: true)
                .Index(t => t.AdID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserAdInformations", "AdID", "dbo.UserAds");
            DropIndex("dbo.UserAdInformations", new[] { "AdID" });
            DropTable("dbo.UserAdInformations");
        }
    }
}
