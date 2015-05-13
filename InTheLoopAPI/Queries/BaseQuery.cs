using InTheLoopAPI.Models;
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
        public DbSet<Event> Events;
        public DbSet<BaseEvent> BaseEvents;
        public DbSet<AttendedEvent> AttendedEvents;

        public BaseQuery()
        {
            Database = new DatabaseContext();
            Users = Database.Users;
            Events = Database.Events;
            BaseEvents = Database.BaseEvents;
            AttendedEvents = Database.AttendedEvents;
        }
    }
}