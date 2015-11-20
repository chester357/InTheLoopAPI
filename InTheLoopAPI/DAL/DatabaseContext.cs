using InTheLoopAPI.Models.Database;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        }

        public DbSet<EventFooter> EventFooters { get; set; }
        public DbSet<EventHeader> EventHeaders { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<ResetToken> ResetTokens { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TagEvent> TagEvents { get; set; }
        public DbSet<TagUser> TagUsers { get; set; }
    }
}
