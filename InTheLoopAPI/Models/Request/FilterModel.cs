using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Models.Request
{
    public class FilterModel
    {
        public bool AllTags { get; set; }

        public List<string> Tags { get; set; }

        public bool AllCosts { get; set; }

        public List<int> Costs { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }
}