namespace Find2Me.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class currency_table_fields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Currencies", "Country", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Currencies", "Country");
        }
    }
}
