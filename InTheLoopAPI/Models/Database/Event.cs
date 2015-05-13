using InTheLoopAPI.Models.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InTheLoopAPI.Models
{
    public class Event
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public int BaseEventId { get; set; }
        public virtual BaseEvent BaseEvent { get; set; }

        public bool Active { get; set; }

        public string City { get; set; }

        public State State { get; set; }

        public int ZipCode { get; set; }

        public int Loops { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public ICollection<AttendedEvent> Attendees { get; set; }
    }
}