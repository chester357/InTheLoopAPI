using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Models.Request
{
    public class FlagModel
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int EventHeaderId { get; set; }

        public string Message { get; set; }

        public int Severity { get; set; }

        public int ReasonId { get; set; }

        public string Reason { get; set; }

        public DateTime Date { get; set; }
    }
}