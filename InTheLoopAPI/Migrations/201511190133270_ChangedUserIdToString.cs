namespace InTheLoopAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedUserIdToString : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.TagUsers", new[] { "User_Id" });
            DropColumn("dbo.TagUsers", "UserId");
            RenameColumn(table: "dbo.TagUsers", name: "User_Id", newName: "UserId");
            AddColumn("dbo.TagUsers", "Tag_Id", c => c.Int());
            AlterColumn("dbo.TagUsers", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.TagUsers", "UserId");
            CreateIndex("dbo.TagUsers", "Tag_Id");
            AddForeignKey("dbo.TagUsers", "Tag_Id", "dbo.Tags", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TagUsers", "Tag_Id", "dbo.Tags");
            DropIndex("dbo.TagUsers", new[] { "Tag_Id" });
            DropIndex("dbo.TagUsers", new[] { "UserId" });
            AlterColumn("dbo.TagUsers", "UserId", c => c.Int(nullable: false));
            DropColumn("dbo.TagUsers", "Tag_Id");
            RenameColumn(table: "dbo.TagUsers", name: "UserId", newName: "User_Id");
            AddColumn("dbo.TagUsers", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.TagUsers", "User_Id");
        }
    }
}
