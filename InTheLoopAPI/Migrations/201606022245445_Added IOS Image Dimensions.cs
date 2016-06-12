namespace InTheLoopAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedIOSImageDimensions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventHeaders", "ImageHeightIOS", c => c.Double(nullable: false));
            AddColumn("dbo.EventHeaders", "ImageWidthIOS", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EventHeaders", "ImageWidthIOS");
            DropColumn("dbo.EventHeaders", "ImageHeightIOS");
        }
    }
}
