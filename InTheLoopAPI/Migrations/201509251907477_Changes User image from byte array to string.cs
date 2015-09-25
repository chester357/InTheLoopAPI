namespace InTheLoopAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesUserimagefrombytearraytostring : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ImageURL", c => c.String());
            DropColumn("dbo.AspNetUsers", "Image");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Image", c => c.Binary());
            DropColumn("dbo.AspNetUsers", "ImageURL");
        }
    }
}
