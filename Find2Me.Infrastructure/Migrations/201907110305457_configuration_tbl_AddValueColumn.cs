namespace Find2Me.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class configuration_tbl_AddValueColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Configurations", "Value", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Configurations", "Value");
        }
    }
}
