namespace InTheLoopAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SupportModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SupportEmail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SupportEmail");
        }
    }
}
