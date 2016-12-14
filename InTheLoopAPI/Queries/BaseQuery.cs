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
        public DbSet<User> MyUsers;
        public DbSet<EventHeader> EventHeaders;
        public DbSet<EventFooter> EventFooters;
        public DbSet<Attendance> Attendances;
        public DbSet<Follow> Follows;
        public DbSet<Loop> Loops;
        public DbSet<UserLoop> UserLoops;
        public DbSet<EventLoop> EventLoops;

        public BaseQuery(DatabaseContext db)
        {
            Database = db;
            MyUsers = Database.MyUsers;
            EventHeaders = Database.EventHeaders;
            EventFooters = Database.EventFooters;
            Attendances = Database.Attendances;
            Follows = Database.Follows;
            Loops = Database.Loops;
            EventLoops = Database.EventLoops;
            UserLoops = Database.UserLoops;
        }
    }
}