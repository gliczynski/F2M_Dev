namespace Find2Me.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationUser_new_fields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "UrlUsername", c => c.String());
            AddColumn("dbo.AspNetUsers", "ProfileImageOriginal", c => c.String());
            AddColumn("dbo.AspNetUsers", "ProfileImageSelected", c => c.String());
            AddColumn("dbo.AspNetUsers", "CreatedOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "UpdatedOn", c => c.DateTime(nullable: false));
            DropColumn("dbo.AspNetUsers", "Hometown");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Hometown", c => c.String());
            DropColumn("dbo.AspNetUsers", "UpdatedOn");
            DropColumn("dbo.AspNetUsers", "CreatedOn");
            DropColumn("dbo.AspNetUsers", "ProfileImageSelected");
            DropColumn("dbo.AspNetUsers", "ProfileImageOriginal");
            DropColumn("dbo.AspNetUsers", "UrlUsername");
        }
    }
}
