using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InTheLoopAPI.Models.Database;

namespace InTheLoopAPI.Models.Database
{
    public class Loop
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public virtual List<EventLoop> EventLoops { get; set; }

        public virtual List<UserLoop> UserLoops { get; set; }
    }
}