using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Models.Request
{
    public class ReviewModel
    {
        public int Id { get; set; }

        public int EventId { get; set; }

        public string Review { get; set; }

        public int Rating { get; set; }

        public bool? Liked { get; set; }

        public List<byte[]> Images { get; set; }
    }
}