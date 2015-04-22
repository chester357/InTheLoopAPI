using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Models.RequestModels
{
    public class RepeatEventModel
    {
        public int BaseEventId { get; set; }

        public bool Active { get; set; }

        public string City { get; set; }

        public int State { get; set; }

        public int ZipCode { get; set; }

        public int Loops { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Category { get; set; }

        public int AgeGroup { get; set; }

        public string Logo { get; set; }

        public string Website { get; set; }
    }
}