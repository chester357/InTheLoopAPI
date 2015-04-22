using InTheLoopAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.DAL
{
    public class InTheLoopContext : DbContext
    {

        public InTheLoopContext(): base("DatabaseContext")
        {

        }

        DbSet<Event> Events { get; set; }
        DbSet<BaseEvent> BaseEvents { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    
    }
}