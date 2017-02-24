namespace InTheLoopAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeFollowNames : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Follow", name: "FollwerId", newName: "FollowingMeId");
            RenameColumn(table: "dbo.Follow", name: "FollowingId", newName: "ImFollowingId");
            RenameIndex(table: "dbo.Follow", name: "IX_FollwerId", newName: "IX_FollowingMeId");
            RenameIndex(table: "dbo.Follow", name: "IX_FollowingId", newName: "IX_ImFollowingId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Follow", name: "IX_ImFollowingId", newName: "IX_FollowingId");
            RenameIndex(table: "dbo.Follow", name: "IX_FollowingMeId", newName: "IX_FollwerId");
            RenameColumn(table: "dbo.Follow", name: "ImFollowingId", newName: "FollowingId");
            RenameColumn(table: "dbo.Follow", name: "FollowingMeId", newName: "FollwerId");
        }
    }
}
