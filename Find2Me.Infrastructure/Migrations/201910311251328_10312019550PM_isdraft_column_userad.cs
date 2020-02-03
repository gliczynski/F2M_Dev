namespace Find2Me.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _10312019550PM_isdraft_column_userad : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserAds", "IsDraft", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserAds", "IsDraft");
        }
    }
}
