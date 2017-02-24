using InTheLoopAPI.Models.Database;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Models
{
    public class User : ApplicationUser
    {
        public String ImageURL { get; set; }

        public string Quote { get; set; }

        public virtual ICollection<Attendance> AttendEvents { get; set; }

        public virtual ICollection<EventFooter> EventFooters { get; set; }

        public virtual ICollection<Follow> Followers { get; set; }

        public virtual ICollection<Follow> Following { get; set; }

        public virtual ICollection<UserLoop> MyLoops { get; set; }

        public virtual ICollection<Loop> CreateLoops { get; set; }

        public virtual ICollection<FlagEvent> FlaggedEvents { get; set; }

    }
}