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
                        Rsvps = c.Int(nullable: false),
                        Views = c.Int(nullable: false),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        ImageURL = c.String(),
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
                "dbo.Loop",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ImageUrl = c.String(),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.EventLoop",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        EventHeaderId = c.Int(nullable: false),
                        LoopId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.EventHeader", t => t.EventHeaderId, cascadeDelete: true)
                .ForeignKey("dbo.Loop", t => t.LoopId, cascadeDelete: true)
                .Index(t => t.EventHeaderId)
                .Index(t => t.LoopId);
            
            CreateTable(
                "dbo.UserLoop",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        LoopId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Loop", t => t.LoopId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.LoopId);
            
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
                        FollwerId = c.String(nullable: false, maxLength: 128),
                        FollowingId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.FollwerId)
                .ForeignKey("dbo.AspNetUsers", t => t.FollowingId)
                .Index(t => t.FollwerId)
                .Index(t => t.FollowingId);
            
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
                        Category = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
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
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.ResetToken", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ReviewImage", "AttendanceId", "dbo.Attendance");
            DropForeignKey("dbo.Follow", "FollowingId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Follow", "FollwerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.FlagEvent", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.FlagEvent", "EventHeaderId", "dbo.EventHeader");
            DropForeignKey("dbo.EventFooter", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserLoop", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserLoop", "LoopId", "dbo.Loop");
            DropForeignKey("dbo.Loop", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.EventLoop", "LoopId", "dbo.Loop");
            DropForeignKey("dbo.EventLoop", "EventHeaderId", "dbo.EventHeader");
            DropForeignKey("dbo.Attendance", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.EventHeader", "EventFooterId", "dbo.EventFooter");
            DropForeignKey("dbo.Attendance", "EventHeaderId", "dbo.EventHeader");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.ResetToken", new[] { "UserId" });
            DropIndex("dbo.ReviewImage", new[] { "AttendanceId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.Follow", new[] { "FollowingId" });
            DropIndex("dbo.Follow", new[] { "FollwerId" });
            DropIndex("dbo.FlagEvent", new[] { "EventHeaderId" });
            DropIndex("dbo.FlagEvent", new[] { "UserId" });
            DropIndex("dbo.UserLoop", new[] { "LoopId" });
            DropIndex("dbo.UserLoop", new[] { "UserId" });
            DropIndex("dbo.EventLoop", new[] { "LoopId" });
            DropIndex("dbo.EventLoop", new[] { "EventHeaderId" });
            DropIndex("dbo.Loop", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.EventFooter", new[] { "UserId" });
            DropIndex("dbo.EventHeader", new[] { "EventFooterId" });
            DropIndex("dbo.Attendance", new[] { "EventHeaderId" });
            DropIndex("dbo.Attendance", new[] { "UserId" });
            DropTable("dbo.SupportEmail");
            DropTable("dbo.StockPhoto");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.ResetToken");
            DropTable("dbo.Contact");
            DropTable("dbo.ReviewImage");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.Follow");
            DropTable("dbo.FlagEvent");
            DropTable("dbo.UserLoop");
            DropTable("dbo.EventLoop");
            DropTable("dbo.Loop");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.EventFooter");
            DropTable("dbo.EventHeader");
            DropTable("dbo.Attendance");
        }
    }
}