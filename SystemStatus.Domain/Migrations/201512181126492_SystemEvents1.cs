namespace SystemStatus.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SystemEvents1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SystemGroups", "IsSystemCritical", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SystemGroups", "IsSystemCritical");
        }
    }
}
