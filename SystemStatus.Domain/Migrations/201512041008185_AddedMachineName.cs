namespace SystemStatus.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedMachineName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppEventHooks", "MachineName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppEventHooks", "MachineName");
        }
    }
}
