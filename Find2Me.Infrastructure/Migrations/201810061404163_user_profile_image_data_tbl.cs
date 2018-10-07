namespace Find2Me.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class user_profile_image_data_tbl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProfileImageDatas",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        CropBoxData_Left = c.Single(nullable: false),
                        CropBoxData_Top = c.Single(nullable: false),
                        CropBoxData_Width = c.Single(nullable: false),
                        CropBoxData_Height = c.Single(nullable: false),
                        CanvasData_Left = c.Single(nullable: false),
                        CanvasData_Top = c.Single(nullable: false),
                        CanvasData_Width = c.Single(nullable: false),
                        CanvasData_Height = c.Single(nullable: false),
                        CanvasData_NaturalWidth = c.Single(nullable: false),
                        CanvasData_NaturalHeight = c.Single(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserProfileImageDatas", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserProfileImageDatas", new[] { "UserId" });
            DropTable("dbo.UserProfileImageDatas");
        }
    }
}
