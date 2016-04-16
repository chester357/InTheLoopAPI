namespace InTheLoopAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedNewFeaturedEventField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventHeaders", "Published", c => c.Boolean(nullable: false));
            AddColumn("dbo.EventHeaders", "Featured", c => c.Boolean(nullable: false));
            AddColumn("dbo.EventHeaders", "TicketUrl", c => c.String());
            AddColumn("dbo.EventHeaders", "OrgContact", c => c.String());
            AddColumn("dbo.EventHeaders", "OrgName", c => c.String());
            AddColumn("dbo.EventHeaders", "OrgUrl", c => c.String());
            AddColumn("dbo.EventHeaders", "VenueContact", c => c.String());
            AddColumn("dbo.EventHeaders", "VenueName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.EventHeaders", "VenueName");
            DropColumn("dbo.EventHeaders", "VenueContact");
            DropColumn("dbo.EventHeaders", "OrgUrl");
            DropColumn("dbo.EventHeaders", "OrgName");
            DropColumn("dbo.EventHeaders", "OrgContact");
            DropColumn("dbo.EventHeaders", "TicketUrl");
            DropColumn("dbo.EventHeaders", "Featured");
            DropColumn("dbo.EventHeaders", "Published");
        }
    }
}
