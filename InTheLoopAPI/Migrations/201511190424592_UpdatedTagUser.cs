namespace InTheLoopAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedTagUser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TagUsers", "EventHeaderId", "dbo.EventHeaders");
            DropForeignKey("dbo.TagUsers", "Tag_Id", "dbo.Tags");
            DropIndex("dbo.TagUsers", new[] { "EventHeaderId" });
            DropIndex("dbo.TagUsers", new[] { "Tag_Id" });
            RenameColumn(table: "dbo.TagUsers", name: "Tag_Id", newName: "TagId");
            AlterColumn("dbo.TagUsers", "TagId", c => c.Int(nullable: false));
            CreateIndex("dbo.TagUsers", "TagId");
            AddForeignKey("dbo.TagUsers", "TagId", "dbo.Tags", "Id", cascadeDelete: true);
            DropColumn("dbo.TagUsers", "EventHeaderId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TagUsers", "EventHeaderId", c => c.Int(nullable: false));
            DropForeignKey("dbo.TagUsers", "TagId", "dbo.Tags");
            DropIndex("dbo.TagUsers", new[] { "TagId" });
            AlterColumn("dbo.TagUsers", "TagId", c => c.Int());
            RenameColumn(table: "dbo.TagUsers", name: "TagId", newName: "Tag_Id");
            CreateIndex("dbo.TagUsers", "Tag_Id");
            CreateIndex("dbo.TagUsers", "EventHeaderId");
            AddForeignKey("dbo.TagUsers", "Tag_Id", "dbo.Tags", "Id");
            AddForeignKey("dbo.TagUsers", "EventHeaderId", "dbo.EventHeaders", "Id", cascadeDelete: true);
        }
    }
}
