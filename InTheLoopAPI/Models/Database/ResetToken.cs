using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Models.Database
{
    public class ResetToken
    {
        public int Id { get; set; }

        public String UserId { get; set; }
        public virtual User User { get; set; }

        public String LongToken { get; set; }

        public String ShortToken { get; set; }
    }
}