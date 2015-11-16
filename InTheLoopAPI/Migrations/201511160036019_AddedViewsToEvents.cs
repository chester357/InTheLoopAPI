namespace InTheLoopAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedViewsToEvents : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventHeaders", "Views", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EventHeaders", "Views");
        }
    }
}
