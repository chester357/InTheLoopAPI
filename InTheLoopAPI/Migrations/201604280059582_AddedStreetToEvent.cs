namespace InTheLoopAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedStreetToEvent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventHeaders", "Street", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.EventHeaders", "Street");
        }
    }
}
