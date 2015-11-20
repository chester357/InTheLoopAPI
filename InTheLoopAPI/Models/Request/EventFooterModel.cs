using InTheLoopAPI.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Models.Request
{
    public class EventFooterModel
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public byte[] Logo { get; set; }

        public string Website { get; set; }

        public AgeGroup AgeGroup { get; set; }
    }
}