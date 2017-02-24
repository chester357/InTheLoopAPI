using InTheLoopAPI.Models.Database;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Models
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser>
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        public DatabaseContext() : base("DatabaseContext")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // the all important base class call! Add this line to make your problems go away.
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Follow>()
                .HasRequired(x => x.FollowingMe)
                .WithMany(m => m.Followers)
                .HasForeignKey(k => k.FollowingMeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Follow>()
                .HasRequired(x => x.ImFollowing)
                .WithMany(m => m.Following)
                .HasForeignKey(k => k.ImFollowingId)
                .WillCascadeOnDelete(false);

            //modelBuilder.Entity<User>()
            //    .HasMany(m => m.Followers)
            //    .WithRequired(r => r.Follower);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public DbSet<EventFooter> EventFooters { get; set; }
        public DbSet<EventHeader> EventHeaders { get; set; }
        public DbSet<User> MyUsers { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<ResetToken> ResetTokens { get; set; }
        public DbSet<Loop> Loops { get; set; }
        public DbSet<EventLoop> EventLoops { get; set; }
        public DbSet<UserLoop> UserLoops { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<FlagEvent> Flags { get; set; }
        public DbSet<StockPhoto> StockPhotos { get; set; }
        public DbSet<SupportEmail> SupportEmails { get; set; }
    }
}
