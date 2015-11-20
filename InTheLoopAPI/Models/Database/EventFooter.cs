using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using InTheLoopAPI.Models.Database;

namespace InTheLoopAPI.Models
{
    public class EventFooter
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Website { get; set; }

        public AgeGroup AgeGroup { get; set; }

        public ICollection<EventHeader> EventHeaders { get; set; }
    }
}