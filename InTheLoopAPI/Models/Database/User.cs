using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Models
{
    public class User : ApplicationUser
    {
        public string Image { get; set; }

        public string Quote { get; set; }

        public ICollection<AttendedEvent> AttendEvents { get; set; }

        public ICollection<Event> Events { get; set; }
    }
}