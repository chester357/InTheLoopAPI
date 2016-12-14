using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InTheLoopAPI.Models.Database;

namespace InTheLoopAPI.Models.Database
{
    public class EventLoop
    {
        public int id { get; set; }

        public int EventHeaderId { get; set; }
        public virtual EventHeader EventHeader { get; set; }

        public int LoopId { get; set; }
        public virtual Loop Loop { get; set; }
    }
}