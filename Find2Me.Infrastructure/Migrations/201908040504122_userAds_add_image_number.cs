namespace Find2Me.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userAds_add_image_number : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserAdImages", "ImageNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserAdImages", "ImageNumber");
        }
    }
}
