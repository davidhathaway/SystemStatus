namespace SystemStatus.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAppEventMessage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppEventMessages",
                c => new
                    {
                        AppEventMessageID = c.Int(nullable: false, identity: true),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.AppEventMessageID);
            
            AddColumn("dbo.AppEvents", "Message_AppEventMessageID", c => c.Int());
            CreateIndex("dbo.AppEvents", "Message_AppEventMessageID");
            AddForeignKey("dbo.AppEvents", "Message_AppEventMessageID", "dbo.AppEventMessages", "AppEventMessageID");
            DropColumn("dbo.AppEvents", "Message");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AppEvents", "Message", c => c.String());
            DropForeignKey("dbo.AppEvents", "Message_AppEventMessageID", "dbo.AppEventMessages");
            DropIndex("dbo.AppEvents", new[] { "Message_AppEventMessageID" });
            DropColumn("dbo.AppEvents", "Message_AppEventMessageID");
            DropTable("dbo.AppEventMessages");
        }
    }
}
