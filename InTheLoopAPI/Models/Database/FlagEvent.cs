using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Models.Database
{
    public class FlagEvent
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public int EventHeaderId { get; set; }
        public virtual EventHeader Event { get; set; }

        public string Message { get; set; }

        public int Severity { get; set; }

        public DateTime Date { get; set; }
    }
}