using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using InTheLoopAPI.Models.Database;

namespace InTheLoopAPI.Models
{
    public class BaseEvent
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Logo { get; set; }

        public string Website { get; set; }

        public AgeGroup AgeGroup { get; set; }

        public Category Category { get; set; }

        public ICollection<Event> Events { get; set; }
    }
}