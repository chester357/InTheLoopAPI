namespace InTheLoopAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedImageUrlToEventHeader : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventHeaders", "ImageURL", c => c.String());
            DropColumn("dbo.EventFooters", "Logo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EventFooters", "Logo", c => c.Binary());
            DropColumn("dbo.EventHeaders", "ImageURL");
        }
    }
}
