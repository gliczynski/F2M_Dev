namespace Find2Me.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userad_tbl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserAds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserAds");
        }
    }
}
