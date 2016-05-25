namespace InTheLoopAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RefactoredTags : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventFooters", "Category", c => c.String());
            AddColumn("dbo.Tags", "IsCategory", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tags", "IsCategory");
            DropColumn("dbo.EventFooters", "Category");
        }
    }
}
