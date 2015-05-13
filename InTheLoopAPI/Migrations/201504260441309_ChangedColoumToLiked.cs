namespace InTheLoopAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedColoumToLiked : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AttendedEvents", "Liked", c => c.Boolean());
            AddColumn("dbo.AttendedEvents", "Image", c => c.String());
            DropColumn("dbo.AttendedEvents", "AttendAgain");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AttendedEvents", "AttendAgain", c => c.Boolean());
            DropColumn("dbo.AttendedEvents", "Image");
            DropColumn("dbo.AttendedEvents", "Liked");
        }
    }
}
