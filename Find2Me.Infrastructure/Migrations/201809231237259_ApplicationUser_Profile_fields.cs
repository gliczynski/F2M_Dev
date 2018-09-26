namespace Find2Me.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationUser_Profile_fields : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Currencies",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 50),
                        Name = c.String(),
                        Symbol = c.String(),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
            
            AddColumn("dbo.AspNetUsers", "FullName", c => c.String());
            AddColumn("dbo.AspNetUsers", "YearOfBirth", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "Sex", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "PreferredCurrency", c => c.String(maxLength: 50));
            AddColumn("dbo.AspNetUsers", "PreferredLanguage", c => c.String());
            CreateIndex("dbo.AspNetUsers", "PreferredCurrency");
            AddForeignKey("dbo.AspNetUsers", "PreferredCurrency", "dbo.Currencies", "Code");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "PreferredCurrency", "dbo.Currencies");
            DropIndex("dbo.AspNetUsers", new[] { "PreferredCurrency" });
            DropColumn("dbo.AspNetUsers", "PreferredLanguage");
            DropColumn("dbo.AspNetUsers", "PreferredCurrency");
            DropColumn("dbo.AspNetUsers", "Sex");
            DropColumn("dbo.AspNetUsers", "YearOfBirth");
            DropColumn("dbo.AspNetUsers", "FullName");
            DropTable("dbo.Currencies");
        }
    }
}
