using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InTheLoopAPI.Models.Database;

namespace InTheLoopAPI.Models.Database
{
    public class TagEvent
    {
        public int id { get; set; }

        public int EventHeaderId { get; set; }
        public virtual EventHeader EventHeader { get; set; }

        public int TagId { get; set; }
        public virtual Tag Tag { get; set; }
    }
}