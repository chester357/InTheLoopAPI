namespace InTheLoopAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamedPropInEventFooter : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.EventHeaders", name: "BaseEventId", newName: "EventFooterId");
            RenameIndex(table: "dbo.EventHeaders", name: "IX_BaseEventId", newName: "IX_EventFooterId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.EventHeaders", name: "IX_EventFooterId", newName: "IX_BaseEventId");
            RenameColumn(table: "dbo.EventHeaders", name: "EventFooterId", newName: "BaseEventId");
        }
    }
}
