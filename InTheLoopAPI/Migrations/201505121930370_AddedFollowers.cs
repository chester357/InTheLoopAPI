namespace InTheLoopAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFollowers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Follows",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FollowerId = c.String(maxLength: 128),
                        FollowieId = c.String(maxLength: 128),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.FollowerId)
                .ForeignKey("dbo.AspNetUsers", t => t.FollowieId)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.FollowerId)
                .Index(t => t.FollowieId)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Follows", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Follows", "FollowieId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Follows", "FollowerId", "dbo.AspNetUsers");
            DropIndex("dbo.Follows", new[] { "User_Id" });
            DropIndex("dbo.Follows", new[] { "FollowieId" });
            DropIndex("dbo.Follows", new[] { "FollowerId" });
            DropTable("dbo.Follows");
        }
    }
}
