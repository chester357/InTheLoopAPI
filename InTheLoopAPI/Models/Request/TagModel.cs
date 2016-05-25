using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Models.Request
{
    public class TagModel
    {
        public String TagName { get; set; }

        public int TagId { get; set; }

        public bool IsCategory { get; set; }

        public int InternalId { get; set; }
    }
}