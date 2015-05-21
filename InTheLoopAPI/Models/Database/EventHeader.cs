using InTheLoopAPI.Models.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace InTheLoopAPI.Models
{
    public class EventHeader
    {
        public int Id { get; set; }

        public int EventFooterId { get; set; }
        public virtual EventFooter EventFooter { get; set; }

        public bool Archived { get; set; }

        public string City { get; set; }

        public State State { get; set; }

        public int ZipCode { get; set; }

        public int Loops { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public ICollection<Attendance> Attendees { get; set; }
    }
}