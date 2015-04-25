namespace InTheLoopAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class temp : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AttendedEvent", "User_Id", "dbo.User");
            DropForeignKey("dbo.Event", "UserId", "dbo.User");
            DropIndex("dbo.Event", new[] { "UserId" });
            DropIndex("dbo.AttendedEvent", new[] { "User_Id" });
            CreateTable(
                "dbo.Profile",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        Image = c.String(),
                        Quote = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Event", "User_Id", c => c.Int());
            AddColumn("dbo.AttendedEvent", "Profile_Id", c => c.Int());
            AlterColumn("dbo.Event", "UserId", c => c.String());
            CreateIndex("dbo.Event", "User_Id");
            CreateIndex("dbo.AttendedEvent", "Profile_Id");
            AddForeignKey("dbo.AttendedEvent", "Profile_Id", "dbo.Profile", "Id");
            AddForeignKey("dbo.Event", "User_Id", "dbo.Profile", "Id");
            DropColumn("dbo.AttendedEvent", "User_Id");
            DropTable("dbo.User");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AttendedEvent", "User_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.Event", "User_Id", "dbo.Profile");
            DropForeignKey("dbo.AttendedEvent", "Profile_Id", "dbo.Profile");
            DropIndex("dbo.AttendedEvent", new[] { "Profile_Id" });
            DropIndex("dbo.Event", new[] { "User_Id" });
            AlterColumn("dbo.Event", "UserId", c => c.String(maxLength: 128));
            DropColumn("dbo.AttendedEvent", "Profile_Id");
            DropColumn("dbo.Event", "User_Id");
            DropTable("dbo.Profile");
            CreateIndex("dbo.AttendedEvent", "User_Id");
            CreateIndex("dbo.Event", "UserId");
            AddForeignKey("dbo.Event", "UserId", "dbo.User", "Id");
            AddForeignKey("dbo.AttendedEvent", "User_Id", "dbo.User", "Id");
        }
    }
}
