namespace InTheLoopAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BaseEvent",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Logo = c.String(),
                        Website = c.String(),
                        AgeGroup = c.Int(nullable: false),
                        Category = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Event",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        BaseEventId = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        City = c.String(),
                        State = c.Int(nullable: false),
                        ZipCode = c.Int(nullable: false),
                        Loops = c.Int(nullable: false),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BaseEvent", t => t.BaseEventId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.BaseEventId);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Event", "UserId", "dbo.User");
            DropForeignKey("dbo.Event", "BaseEventId", "dbo.BaseEvent");
            DropForeignKey("dbo.AttendedEvent", "User_Id", "dbo.User");
            DropForeignKey("dbo.AttendedEvent", "EventId", "dbo.Event");
            DropIndex("dbo.AttendedEvent", new[] { "User_Id" });
            DropIndex("dbo.AttendedEvent", new[] { "EventId" });
            DropIndex("dbo.Event", new[] { "BaseEventId" });
            DropIndex("dbo.Event", new[] { "UserId" });
            DropTable("dbo.User");
            DropTable("dbo.AttendedEvent");
            DropTable("dbo.Event");
            DropTable("dbo.BaseEvent");
        }
    }
}
