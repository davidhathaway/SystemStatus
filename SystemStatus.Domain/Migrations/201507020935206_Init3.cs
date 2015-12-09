namespace SystemStatus.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppEventHooks", "FastStatusLimit", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.AppEventHooks", "NormalStatusLimit", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.AppEventHooks", "SlowStatusLimit", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppEventHooks", "SlowStatusLimit");
            DropColumn("dbo.AppEventHooks", "NormalStatusLimit");
            DropColumn("dbo.AppEventHooks", "FastStatusLimit");
        }
    }
}
