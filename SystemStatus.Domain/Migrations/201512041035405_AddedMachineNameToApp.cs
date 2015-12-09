namespace SystemStatus.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedMachineNameToApp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Apps", "MachineName", c => c.String());
            DropColumn("dbo.AppEventHooks", "MachineName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AppEventHooks", "MachineName", c => c.String());
            DropColumn("dbo.Apps", "MachineName");
        }
    }
}
