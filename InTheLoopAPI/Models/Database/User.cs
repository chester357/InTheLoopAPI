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

        public virtual ICollection<TagUser> Tags { get; set; }

    }
}