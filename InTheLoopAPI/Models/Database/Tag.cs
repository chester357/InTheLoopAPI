using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InTheLoopAPI.Models.Database;

namespace InTheLoopAPI.Models.Database
{
    public class Tag
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual List<TagEvent> TagEvents { get; set; }

        public virtual List<TagUser> TagUsers { get; set; }

        public bool IsCategory { get; set; }
    }
}