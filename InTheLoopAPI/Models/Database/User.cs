﻿using InTheLoopAPI.Models.Database;
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

        public virtual ICollection<AttendedEvent> AttendEvents { get; set; }

        public virtual ICollection<EventHeader> Events { get; set; }

        public virtual ICollection<Follow> Followers { get; set; }
    }
}