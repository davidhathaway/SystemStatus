namespace SystemStatus.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppEventHooks",
                c => new
                    {
                        AppEventHookID = c.Int(nullable: false, identity: true),
                        AppID = c.Int(nullable: false),
                        AppEventHookTypeID = c.Int(nullable: false),
                        Name = c.String(),
                        Command = c.String(),
                        Script = c.String(),
                        HttpUrl = c.String(),
                    })
                .PrimaryKey(t => t.AppEventHookID)
                .ForeignKey("dbo.Apps", t => t.AppID, cascadeDelete: true)
                .ForeignKey("dbo.AppEventHookTypes", t => t.AppEventHookTypeID, cascadeDelete: true)
                .Index(t => t.AppID)
                .Index(t => t.AppEventHookTypeID);
            
            CreateTable(
                "dbo.Apps",
                c => new
                    {
                        AppID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.AppID);
            
            CreateTable(
                "dbo.AppEvents",
                c => new
                    {
                        AppEventID = c.Int(nullable: false, identity: true),
                        EventTime = c.DateTime(nullable: false),
                        Message = c.String(),
                        AppID = c.Int(nullable: false),
                        AppStatus = c.Int(nullable: false),
                        FromAppEventHookID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AppEventID)
                .ForeignKey("dbo.Apps", t => t.AppID, cascadeDelete: true)
                .Index(t => t.AppID);
            
            CreateTable(
                "dbo.AppEventHookTypes",
                c => new
                    {
                        AppEventHookTypeID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.AppEventHookTypeID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AppEventHooks", "AppEventHookTypeID", "dbo.AppEventHookTypes");
            DropForeignKey("dbo.AppEventHooks", "AppID", "dbo.Apps");
            DropForeignKey("dbo.AppEvents", "AppID", "dbo.Apps");
            DropIndex("dbo.AppEvents", new[] { "AppID" });
            DropIndex("dbo.AppEventHooks", new[] { "AppEventHookTypeID" });
            DropIndex("dbo.AppEventHooks", new[] { "AppID" });
            DropTable("dbo.AppEventHookTypes");
            DropTable("dbo.AppEvents");
            DropTable("dbo.Apps");
            DropTable("dbo.AppEventHooks");
        }
    }
}
