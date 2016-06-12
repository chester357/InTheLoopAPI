namespace InTheLoopAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedStockImageTable2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StockPhotoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImageURL = c.String(),
                        HeightPx = c.Double(nullable: false),
                        WidthPx = c.Double(nullable: false),
                        Category = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.StockPhotoes");
        }
    }
}
