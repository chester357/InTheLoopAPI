namespace InTheLoopAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedFlagEvent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FlagEvents", "ReasonId", c => c.Int(nullable: false));
            AddColumn("dbo.FlagEvents", "Reason", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FlagEvents", "Reason");
            DropColumn("dbo.FlagEvents", "ReasonId");
        }
    }
}
