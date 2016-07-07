namespace InTheLoopAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attendance",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        EventHeaderId = c.Int(nullable: false),
                        Review = c.String(),
                        Rating = c.Int(nullable: false),
                        Liked = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EventHeader", t => t.EventHeaderId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.EventHeaderId);
            
            CreateTable(
                "dbo.EventHeader",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EventFooterId = c.Int(nullable: false),
                        Archived = c.Boolean(nullable: false),
                        Street = c.String(),
                        City = c.String(),
                        State = c.Int(nullable: false),
                        ZipCode = c.Int(nullable: false),
                        Loops = c.Int(nullable: false),
                        Views = c.Int(nullable: false),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        ImageURL = c.String(),
                        ImageHeightPx = c.Double(nullable: false),
                        ImageWidthPx = c.Double(nullable: false),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        Price = c.Int(nullable: false),
                        Published = c.Boolean(nullable: false),
                        Featured = c.Boolean(nullable: false),
                        TicketUrl = c.String(),
                        OrgContact = c.String(),
                        OrgName = c.String(),
                        OrgUrl = c.String(),
                        VenueContact = c.String(),
                        VenueName = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EventFooter", t => t.EventFooterId, cascadeDelete: true)
                .Index(t => t.EventFooterId);
            
            CreateTable(
                "dbo.EventFooter",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        Title = c.String(),
                        Description = c.String(),
                        Website = c.String(),
                        AgeGroup = c.Int(nullable: false),
                        Category = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        ImageURL = c.String(),
                        Quote = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.FlagEvent",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        EventHeaderId = c.Int(nullable: false),
                        Message = c.String(),
                        Severity = c.Int(nullable: false),
                        ReasonId = c.Int(nullable: false),
                        Reason = c.String(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EventHeader", t => t.EventHeaderId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.EventHeaderId);
            
            CreateTable(
                "dbo.Follow",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        FollowingId = c.String(maxLength: 128),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.FollowingId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.UserId)
                .Index(t => t.FollowingId)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.TagUser",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        TagId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Tag", t => t.TagId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.TagId);
            
            CreateTable(
                "dbo.Tag",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsCategory = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TagEvent",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        EventHeaderId = c.Int(nullable: false),
                        TagId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.EventHeader", t => t.EventHeaderId, cascadeDelete: true)
                .ForeignKey("dbo.Tag", t => t.TagId, cascadeDelete: true)
                .Index(t => t.EventHeaderId)
                .Index(t => t.TagId);
            
            CreateTable(
                "dbo.ReviewImage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AttendanceId = c.Int(nullable: false),
                        Image = c.Binary(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Attendance", t => t.AttendanceId, cascadeDelete: true)
                .Index(t => t.AttendanceId);
            
            CreateTable(
                "dbo.Contact",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ResetToken",
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
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.StockPhoto",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImageURL = c.String(),
                        HeightPx = c.Double(nullable: false),
                        WidthPx = c.Double(nullable: false),
                        Category = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.ResetToken", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ReviewImage", "AttendanceId", "dbo.Attendance");
            DropForeignKey("dbo.TagUser", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.TagUser", "TagId", "dbo.Tag");
            DropForeignKey("dbo.TagEvent", "TagId", "dbo.Tag");
            DropForeignKey("dbo.TagEvent", "EventHeaderId", "dbo.EventHeader");
            DropForeignKey("dbo.Follow", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Follow", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Follow", "FollowingId", "dbo.AspNetUsers");
            DropForeignKey("dbo.FlagEvent", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.FlagEvent", "EventHeaderId", "dbo.EventHeader");
            DropForeignKey("dbo.EventFooter", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Attendance", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.EventHeader", "EventFooterId", "dbo.EventFooter");
            DropForeignKey("dbo.Attendance", "EventHeaderId", "dbo.EventHeader");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.ResetToken", new[] { "UserId" });
            DropIndex("dbo.ReviewImage", new[] { "AttendanceId" });
            DropIndex("dbo.TagEvent", new[] { "TagId" });
            DropIndex("dbo.TagEvent", new[] { "EventHeaderId" });
            DropIndex("dbo.TagUser", new[] { "TagId" });
            DropIndex("dbo.TagUser", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.Follow", new[] { "User_Id" });
            DropIndex("dbo.Follow", new[] { "FollowingId" });
            DropIndex("dbo.Follow", new[] { "UserId" });
            DropIndex("dbo.FlagEvent", new[] { "EventHeaderId" });
            DropIndex("dbo.FlagEvent", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.EventFooter", new[] { "UserId" });
            DropIndex("dbo.EventHeader", new[] { "EventFooterId" });
            DropIndex("dbo.Attendance", new[] { "EventHeaderId" });
            DropIndex("dbo.Attendance", new[] { "UserId" });
            DropTable("dbo.StockPhoto");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.ResetToken");
            DropTable("dbo.Contact");
            DropTable("dbo.ReviewImage");
            DropTable("dbo.TagEvent");
            DropTable("dbo.Tag");
            DropTable("dbo.TagUser");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.Follow");
            DropTable("dbo.FlagEvent");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.EventFooter");
            DropTable("dbo.EventHeader");
            DropTable("dbo.Attendance");
        }
    }
}
