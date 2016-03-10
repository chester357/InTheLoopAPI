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

        public int Views { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string ImageURL { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public int Price { get; set; }

        public bool Published { get; set; }

        public bool Featured { get; set; }

        public string TicketUrl { get; set; }

        public string OrgContact { get; set; }

        public string OrgName { get; set; }

        public string OrgUrl { get; set; }

        public string VenueContact { get; set; }

        public string VenueName { get; set; }

        public ICollection<Attendance> Attendees { get; set; }

        public ICollection<TagEvent> TagEvents { get; set; }
    }
}