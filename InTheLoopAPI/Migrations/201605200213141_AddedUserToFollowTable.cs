namespace InTheLoopAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUserToFollowTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Follows", "UserId", "dbo.AspNetUsers");
            AddColumn("dbo.Follows", "User_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Follows", "FollowingId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Follows", "FollowingId");
            CreateIndex("dbo.Follows", "User_Id");
            AddForeignKey("dbo.Follows", "FollowingId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Follows", "User_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Follows", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Follows", "FollowingId", "dbo.AspNetUsers");
            DropIndex("dbo.Follows", new[] { "User_Id" });
            DropIndex("dbo.Follows", new[] { "FollowingId" });
            AlterColumn("dbo.Follows", "FollowingId", c => c.String());
            DropColumn("dbo.Follows", "User_Id");
            AddForeignKey("dbo.Follows", "UserId", "dbo.AspNetUsers", "Id");
        }
    }
}
