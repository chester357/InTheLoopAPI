using InTheLoopAPI.Models;
using InTheLoopAPI.Models.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Queries
{
    public class BaseQuery
    {
        public DatabaseContext Database;
        public DbSet<User> Users;
        public DbSet<EventHeader> EventHeaders;
        public DbSet<EventFooter> EventFooters;
        public DbSet<Attendance> Attendances;
        public DbSet<Follow> Follows;
        public DbSet<Tag> Tags;
        public DbSet<TagUser> TagUsers;
        public DbSet<TagEvent> TagEvents;

        public BaseQuery(DatabaseContext db)
        {
            Database = db;
            Users = Database.Users;
            EventHeaders = Database.EventHeaders;
            EventFooters = Database.EventFooters;
            Attendances = Database.Attendances;
            Follows = Database.Follows;
            Tags = Database.Tags;
            TagEvents = Database.TagEvents;
            TagUsers = Database.TagUsers;
        }
    }
}