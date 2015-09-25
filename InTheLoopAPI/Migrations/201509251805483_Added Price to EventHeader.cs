namespace InTheLoopAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPricetoEventHeader : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventHeaders", "Price", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EventHeaders", "Price");
        }
    }
}
