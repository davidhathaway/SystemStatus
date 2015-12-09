namespace SystemStatus.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init5 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AppEvents", "Value", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AppEvents", "Value", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
