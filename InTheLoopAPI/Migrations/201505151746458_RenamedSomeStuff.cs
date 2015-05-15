namespace InTheLoopAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamedSomeStuff : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AttendedEvents", newName: "Attendances");
            RenameTable(name: "dbo.Events", newName: "EventHeaders");
            RenameTable(name: "dbo.BaseEvents", newName: "EventFooters");
            RenameColumn(table: "dbo.Attendances", name: "EventId", newName: "EventHeaderId");
            RenameIndex(table: "dbo.Attendances", name: "IX_EventId", newName: "IX_EventHeaderId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Attendances", name: "IX_EventHeaderId", newName: "IX_EventId");
            RenameColumn(table: "dbo.Attendances", name: "EventHeaderId", newName: "EventId");
            RenameTable(name: "dbo.EventFooters", newName: "BaseEvents");
            RenameTable(name: "dbo.EventHeaders", newName: "Events");
            RenameTable(name: "dbo.Attendances", newName: "AttendedEvents");
        }
    }
}
