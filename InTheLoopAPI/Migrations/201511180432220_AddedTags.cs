namespace InTheLoopAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTags : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TagEvents",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        EventHeaderId = c.Int(nullable: false),
                        TagId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.EventHeaders", t => t.EventHeaderId, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.TagId, cascadeDelete: true)
                .Index(t => t.EventHeaderId)
                .Index(t => t.TagId);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TagUsers",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        EventHeaderId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.EventHeaders", t => t.EventHeaderId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.EventHeaderId)
                .Index(t => t.User_Id);
            
            DropColumn("dbo.EventFooters", "Category");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EventFooters", "Category", c => c.Int(nullable: false));
            DropForeignKey("dbo.TagUsers", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.TagUsers", "EventHeaderId", "dbo.EventHeaders");
            DropForeignKey("dbo.TagEvents", "TagId", "dbo.Tags");
            DropForeignKey("dbo.TagEvents", "EventHeaderId", "dbo.EventHeaders");
            DropIndex("dbo.TagUsers", new[] { "User_Id" });
            DropIndex("dbo.TagUsers", new[] { "EventHeaderId" });
            DropIndex("dbo.TagEvents", new[] { "TagId" });
            DropIndex("dbo.TagEvents", new[] { "EventHeaderId" });
            DropTable("dbo.TagUsers");
            DropTable("dbo.Tags");
            DropTable("dbo.TagEvents");
        }
    }
}
