namespace InTheLoopAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedStockImageTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventHeaders", "ImageHeightPx", c => c.Double(nullable: false));
            AddColumn("dbo.EventHeaders", "ImageWidthPx", c => c.Double(nullable: false));
            DropColumn("dbo.EventHeaders", "ImageHeightIOS");
            DropColumn("dbo.EventHeaders", "ImageWidthIOS");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EventHeaders", "ImageWidthIOS", c => c.Double(nullable: false));
            AddColumn("dbo.EventHeaders", "ImageHeightIOS", c => c.Double(nullable: false));
            DropColumn("dbo.EventHeaders", "ImageWidthPx");
            DropColumn("dbo.EventHeaders", "ImageHeightPx");
        }
    }
}
