namespace SystemStatus.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init4 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AppEventHooks", "SlowStatusLimit");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AppEventHooks", "SlowStatusLimit", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
