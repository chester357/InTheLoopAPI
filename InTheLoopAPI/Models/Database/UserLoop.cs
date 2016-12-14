using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InTheLoopAPI.Models.Database;

namespace InTheLoopAPI.Models.Database
{
    public class UserLoop
    {
        public int id { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public int LoopId { get; set; }
        public virtual Loop Loop { get; set; }
    }
}