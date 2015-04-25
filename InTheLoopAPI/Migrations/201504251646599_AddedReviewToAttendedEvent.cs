namespace InTheLoopAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedReviewToAttendedEvent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AttendedEvents", "Review", c => c.String());
            AddColumn("dbo.AttendedEvents", "Rating", c => c.Int(nullable: false));
            AddColumn("dbo.AttendedEvents", "AttendAgain", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AttendedEvents", "AttendAgain");
            DropColumn("dbo.AttendedEvents", "Rating");
            DropColumn("dbo.AttendedEvents", "Review");
        }
    }
}
