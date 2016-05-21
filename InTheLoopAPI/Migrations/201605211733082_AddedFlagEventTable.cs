namespace InTheLoopAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFlagEventTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FlagEvents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        EventHeaderId = c.Int(nullable: false),
                        Message = c.String(),
                        Severity = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EventHeaders", t => t.EventHeaderId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.EventHeaderId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FlagEvents", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.FlagEvents", "EventHeaderId", "dbo.EventHeaders");
            DropIndex("dbo.FlagEvents", new[] { "EventHeaderId" });
            DropIndex("dbo.FlagEvents", new[] { "UserId" });
            DropTable("dbo.FlagEvents");
        }
    }
}
