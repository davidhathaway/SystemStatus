namespace SystemStatus.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SystemEvents : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SystemEvents",
                c => new
                    {
                        SystemEventID = c.Int(nullable: false, identity: true),
                        SystemGroupID = c.Int(nullable: false),
                        EventTime = c.DateTime(nullable: false),
                        IsDown = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SystemEventID)
                .ForeignKey("dbo.SystemGroups", t => t.SystemGroupID, cascadeDelete: true)
                .Index(t => t.SystemGroupID);
            
            AddColumn("dbo.Apps", "IsSystemCritical", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SystemEvents", "SystemGroupID", "dbo.SystemGroups");
            DropIndex("dbo.SystemEvents", new[] { "SystemGroupID" });
            DropColumn("dbo.Apps", "IsSystemCritical");
            DropTable("dbo.SystemEvents");
        }
    }
}
