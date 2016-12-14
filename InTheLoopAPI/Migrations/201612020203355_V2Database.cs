namespace InTheLoopAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V2Database : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TagEvent", "EventHeaderId", "dbo.EventHeader");
            DropForeignKey("dbo.TagEvent", "TagId", "dbo.Tag");
            DropForeignKey("dbo.TagUser", "TagId", "dbo.Tag");
            DropForeignKey("dbo.TagUser", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.TagUser", new[] { "UserId" });
            DropIndex("dbo.TagUser", new[] { "TagId" });
            DropIndex("dbo.TagEvent", new[] { "EventHeaderId" });
            DropIndex("dbo.TagEvent", new[] { "TagId" });
            CreateTable(
                "dbo.Loop",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ImageUrl = c.String(),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.EventLoop",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        EventHeaderId = c.Int(nullable: false),
                        LoopId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.EventHeader", t => t.EventHeaderId, cascadeDelete: true)
                .ForeignKey("dbo.Loop", t => t.LoopId, cascadeDelete: true)
                .Index(t => t.EventHeaderId)
                .Index(t => t.LoopId);
            
            CreateTable(
                "dbo.UserLoop",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        LoopId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Loop", t => t.LoopId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.LoopId);
            
            AddColumn("dbo.EventHeader", "Rsvps", c => c.Int(nullable: false));
            DropColumn("dbo.EventHeader", "Loops");
            DropColumn("dbo.EventHeader", "ImageHeightPx");
            DropColumn("dbo.EventHeader", "ImageWidthPx");
            DropColumn("dbo.EventFooter", "AgeGroup");
            DropColumn("dbo.EventFooter", "Category");
            DropColumn("dbo.StockPhoto", "HeightPx");
            DropColumn("dbo.StockPhoto", "WidthPx");
            DropTable("dbo.TagUser");
            DropTable("dbo.Tag");
            DropTable("dbo.TagEvent");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TagEvent",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        EventHeaderId = c.Int(nullable: false),
                        TagId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Tag",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsCategory = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TagUser",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        TagId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            AddColumn("dbo.StockPhoto", "WidthPx", c => c.Double(nullable: false));
            AddColumn("dbo.StockPhoto", "HeightPx", c => c.Double(nullable: false));
            AddColumn("dbo.EventFooter", "Category", c => c.String());
            AddColumn("dbo.EventFooter", "AgeGroup", c => c.Int(nullable: false));
            AddColumn("dbo.EventHeader", "ImageWidthPx", c => c.Double(nullable: false));
            AddColumn("dbo.EventHeader", "ImageHeightPx", c => c.Double(nullable: false));
            AddColumn("dbo.EventHeader", "Loops", c => c.Int(nullable: false));
            DropForeignKey("dbo.UserLoop", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserLoop", "LoopId", "dbo.Loop");
            DropForeignKey("dbo.Loop", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.EventLoop", "LoopId", "dbo.Loop");
            DropForeignKey("dbo.EventLoop", "EventHeaderId", "dbo.EventHeader");
            DropIndex("dbo.UserLoop", new[] { "LoopId" });
            DropIndex("dbo.UserLoop", new[] { "UserId" });
            DropIndex("dbo.EventLoop", new[] { "LoopId" });
            DropIndex("dbo.EventLoop", new[] { "EventHeaderId" });
            DropIndex("dbo.Loop", new[] { "UserId" });
            DropColumn("dbo.EventHeader", "Rsvps");
            DropTable("dbo.UserLoop");
            DropTable("dbo.EventLoop");
            DropTable("dbo.Loop");
            CreateIndex("dbo.TagEvent", "TagId");
            CreateIndex("dbo.TagEvent", "EventHeaderId");
            CreateIndex("dbo.TagUser", "TagId");
            CreateIndex("dbo.TagUser", "UserId");
            AddForeignKey("dbo.TagUser", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.TagUser", "TagId", "dbo.Tag", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TagEvent", "TagId", "dbo.Tag", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TagEvent", "EventHeaderId", "dbo.EventHeader", "Id", cascadeDelete: true);
        }
    }
}
