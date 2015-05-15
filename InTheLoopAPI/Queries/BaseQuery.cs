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
        public DbSet<EventHeader> Events;
        public DbSet<EventFooter> BaseEvents;
        public DbSet<AttendedEvent> AttendedEvents;
        public DbSet<Follow> Follows;

        public BaseQuery()
        {
            Database = new DatabaseContext();
            Users = Database.Users;
            Events = Database.Events;
            BaseEvents = Database.BaseEvents;
            AttendedEvents = Database.AttendedEvents;
            Follows = Database.Follows;
        }
    }
}