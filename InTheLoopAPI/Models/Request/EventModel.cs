using InTheLoopAPI.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Models.RequestModels
{
    public class EventModel
    {
        public int Id { get; set; }

        public int BaseEventId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Category Category { get; set; }   

        public AgeGroup AgeGroup { get; set; }

        public string Logo { get; set; }

        public string Website { get; set; }

        public string UserId { get; set; }

        public bool Active { get; set; }

        public string City { get; set; }

        public State State { get; set; }

        public int ZipCode { get; set; }

        public int Loops { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }
}