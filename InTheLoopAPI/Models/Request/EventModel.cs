using InTheLoopAPI.Models.Database;
using InTheLoopAPI.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Models.RequestModels
{
    public class EventModel
    {
        public int Id { get; set; }

        public int EventFooterId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public AgeGroup AgeGroup { get; set; }

        public string EventImageURL { get; set; }

        public string Website { get; set; }

        public string UserId { get; set; }

        public string UserProfileURL { get; set; }

        public bool Active { get; set; }

        public string City { get; set; }

        public State State { get; set; }

        public int ZipCode { get; set; }

        public int Loops { get; set; }

        public int Views { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public int Price { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public List<TagModel> Tags { get; set; }
    }
}