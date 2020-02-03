namespace Find2Me.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class useradinformation_categoryId_foreign_key_category_tbl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.UserAdInformations", "CategoryID", c => c.Int(nullable: false));
            CreateIndex("dbo.UserAdInformations", "CategoryID");
            AddForeignKey("dbo.UserAdInformations", "CategoryID", "dbo.Categories", "Id", cascadeDelete: true);
            DropColumn("dbo.UserAdInformations", "Category");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserAdInformations", "Category", c => c.String());
            DropForeignKey("dbo.UserAdInformations", "CategoryID", "dbo.Categories");
            DropIndex("dbo.UserAdInformations", new[] { "CategoryID" });
            DropColumn("dbo.UserAdInformations", "CategoryID");
            DropTable("dbo.Categories");
        }
    }
}
