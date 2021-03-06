﻿using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Models
{
    public class Attendance
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public int EventHeaderId { get; set; }
        public virtual EventHeader Event { get; set; }

        public string Review { get; set; }

        public int Rating { get; set; }

        public bool? Liked { get; set; }

        public virtual ICollection<ReviewImage> ReviewImages { get; set; }
    }
}