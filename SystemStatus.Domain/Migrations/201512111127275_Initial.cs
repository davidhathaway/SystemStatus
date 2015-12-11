namespace SystemStatus.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppEventHookTypes",
                c => new
                    {
                        AppEventHookTypeID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.AppEventHookTypeID);
            
            CreateTable(
                "dbo.AppEvents",
                c => new
                    {
                        AppEventID = c.Int(nullable: false, identity: true),
                        EventTime = c.DateTime(nullable: false),
                        Message = c.String(),
                        AppID = c.Int(nullable: false),
                        AppStatus = c.Int(nullable: false),
                        Value = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.AppEventID)
                .ForeignKey("dbo.Apps", t => t.AppID, cascadeDelete: true)
                .Index(t => t.AppID);
            
            CreateTable(
                "dbo.Apps",
                c => new
                    {
                        AppID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        AgentName = c.String(),
                        SystemGroupID = c.Int(nullable: false),
                        AppEventHookTypeID = c.Int(nullable: false),
                        Command = c.String(),
                        Script = c.String(),
                        HttpUrl = c.String(),
                        Active = c.Boolean(nullable: false),
                        FastStatusLimit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NormalStatusLimit = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.AppID)
                .ForeignKey("dbo.AppEventHookTypes", t => t.AppEventHookTypeID, cascadeDelete: true)
                .ForeignKey("dbo.SystemGroups", t => t.SystemGroupID, cascadeDelete: true)
                .Index(t => t.SystemGroupID)
                .Index(t => t.AppEventHookTypeID);
            
            CreateTable(
                "dbo.SystemGroups",
                c => new
                    {
                        SystemGroupID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ParentID = c.Int(),
                    })
                .PrimaryKey(t => t.SystemGroupID)
                .ForeignKey("dbo.SystemGroups", t => t.ParentID)
                .Index(t => t.ParentID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SystemGroups", "ParentID", "dbo.SystemGroups");
            DropForeignKey("dbo.Apps", "SystemGroupID", "dbo.SystemGroups");
            DropForeignKey("dbo.AppEvents", "AppID", "dbo.Apps");
            DropForeignKey("dbo.Apps", "AppEventHookTypeID", "dbo.AppEventHookTypes");
            DropIndex("dbo.SystemGroups", new[] { "ParentID" });
            DropIndex("dbo.Apps", new[] { "AppEventHookTypeID" });
            DropIndex("dbo.Apps", new[] { "SystemGroupID" });
            DropIndex("dbo.AppEvents", new[] { "AppID" });
            DropTable("dbo.SystemGroups");
            DropTable("dbo.Apps");
            DropTable("dbo.AppEvents");
            DropTable("dbo.AppEventHookTypes");
        }
    }
}
