using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Models
{
    public class Profile
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string Image { get; set; }

        public string Quote { get; set; }

        public ICollection<AttendedEvent> AttendEvents { get; set; }
    }
}