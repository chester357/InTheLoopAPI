namespace InTheLoopAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ResetTokenTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ResetTokens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        LongToken = c.String(),
                        ShortToken = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ResetTokens", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.ResetTokens", new[] { "UserId" });
            DropTable("dbo.ResetTokens");
        }
    }
}
