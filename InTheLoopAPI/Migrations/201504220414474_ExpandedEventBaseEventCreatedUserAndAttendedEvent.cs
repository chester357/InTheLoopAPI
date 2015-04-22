namespace InTheLoopAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExpandedEventBaseEventCreatedUserAndAttendedEvent : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Event", "BaseEvent_ID", "dbo.BaseEvent");
            DropIndex("dbo.Event", new[] { "BaseEvent_ID" });
            RenameColumn(table: "dbo.Event", name: "BaseEvent_ID", newName: "BaseEventId");
            CreateTable(
                "dbo.AttendedEvent",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        EventId = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Event", t => t.EventId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.User_Id)
                .Index(t => t.EventId)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.BaseEvent", "Category", c => c.Int(nullable: false));
            AddColumn("dbo.BaseEvent", "AgeGroup", c => c.Int(nullable: false));
            AddColumn("dbo.BaseEvent", "Logo", c => c.String());
            AddColumn("dbo.BaseEvent", "Website", c => c.String());
            AddColumn("dbo.Event", "UserId", c => c.String(maxLength: 128));
            AddColumn("dbo.Event", "Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.Event", "City", c => c.String());
            AddColumn("dbo.Event", "State", c => c.Int(nullable: false));
            AddColumn("dbo.Event", "ZipCode", c => c.Int(nullable: false));
            AddColumn("dbo.Event", "Loops", c => c.Int(nullable: false));
            AddColumn("dbo.Event", "Latitude", c => c.Double(nullable: false));
            AddColumn("dbo.Event", "Longitude", c => c.Double(nullable: false));
            AlterColumn("dbo.Event", "BaseEventId", c => c.Int(nullable: false));
            CreateIndex("dbo.Event", "UserId");
            CreateIndex("dbo.Event", "BaseEventId");
            AddForeignKey("dbo.Event", "UserId", "dbo.User", "Id");
            AddForeignKey("dbo.Event", "BaseEventId", "dbo.BaseEvent", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Event", "BaseEventId", "dbo.BaseEvent");
            DropForeignKey("dbo.Event", "UserId", "dbo.User");
            DropForeignKey("dbo.AttendedEvent", "User_Id", "dbo.User");
            DropForeignKey("dbo.AttendedEvent", "EventId", "dbo.Event");
            DropIndex("dbo.AttendedEvent", new[] { "User_Id" });
            DropIndex("dbo.AttendedEvent", new[] { "EventId" });
            DropIndex("dbo.Event", new[] { "BaseEventId" });
            DropIndex("dbo.Event", new[] { "UserId" });
            AlterColumn("dbo.Event", "BaseEventId", c => c.Int());
            DropColumn("dbo.Event", "Longitude");
            DropColumn("dbo.Event", "Latitude");
            DropColumn("dbo.Event", "Loops");
            DropColumn("dbo.Event", "ZipCode");
            DropColumn("dbo.Event", "State");
            DropColumn("dbo.Event", "City");
            DropColumn("dbo.Event", "Active");
            DropColumn("dbo.Event", "UserId");
            DropColumn("dbo.BaseEvent", "Website");
            DropColumn("dbo.BaseEvent", "Logo");
            DropColumn("dbo.BaseEvent", "AgeGroup");
            DropColumn("dbo.BaseEvent", "Category");
            DropTable("dbo.User");
            DropTable("dbo.AttendedEvent");
            RenameColumn(table: "dbo.Event", name: "BaseEventId", newName: "BaseEvent_ID");
            CreateIndex("dbo.Event", "BaseEvent_ID");
            AddForeignKey("dbo.Event", "BaseEvent_ID", "dbo.BaseEvent", "ID");
        }
    }
}
