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

        public string EventImageURL { get; set; }

        public string Website { get; set; }

        public string UserId { get; set; }

        public UserModel User { get; set; }

        public string UserProfileURL { get; set; }

        public bool Active { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public State State { get; set; }

        public int ZipCode { get; set; }

        public int Rsvps { get; set; }

        public int Views { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public int Price { get; set; }

        public bool IsAttending { get; set; }

        public bool Published { get; set; }

        public bool Featured { get; set; }

        public string TicketUrl { get; set; }

        public string OrgContact { get; set; }

        public string OrgName { get; set; }

        public string OrgUrl { get; set; }

        public string VenueContact { get; set; }

        public string VenueName { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public List<LoopModel> Loops { get; set; }

        public EventHeader ToEventHeader(string userId)
        {
            var eventHeader = new EventHeader
            {
                Id = this.Id,
                EventFooterId = this.EventFooterId,
                EventFooter = new EventFooter
                {
                    //public int Id { get; set; }
                    Id = this.EventFooterId,
                    //public string UserId { get; set; }
                    UserId = userId,
                    //public string Title { get; set; }
                    Title = this.Title,
                    //public string Description { get; set; }
                    Description = this.Description,
                    //public string Website { get; set; }
                    Website = this.Website
                },
                //public bool Archived { get; set; }
                Archived = this.Active,
                //public string City { get; set; }
                City = this.City,
                Street = this.Street,
                //public State State { get; set; }
                State = this.State,
                //public int ZipCode { get; set; }
                ZipCode = this.ZipCode,
                //public int Loops { get; set; }
                Rsvps = this.Rsvps,
                //public int Views { get; set; }
                Views = this.Views,
                //public double Latitude { get; set; }
                Latitude = this.Latitude,
                //public double Longitude { get; set; }
                Longitude = this.Longitude,
                //public string ImageURL { get; set; }
                ImageURL = this.EventImageURL,
                //public DateTime Start { get; set; }

                /*
                    This is so people can't create events in the past
                */
                Start = this.Start.CompareTo(DateTime.UtcNow) < 0 ? DateTime.UtcNow : this.Start,
                //public DateTime End { get; set; }
                End = this.End.CompareTo(DateTime.UtcNow) < 0 ? DateTime.UtcNow : this.End,

                //public int Price { get; set; }
                Price = this.Price,
                //public bool Published { get; set; }
                Published = this.Published,
                //public bool Featured { get; set; }
                Featured = this.Featured,
                //public string TicketUrl { get; set; }
                TicketUrl = this.TicketUrl,
                //public string OrgContact { get; set; }
                OrgContact = this.OrgContact,
                //public string OrgName { get; set; }
                OrgName = this.OrgName,
                //public string OrgUrl { get; set; }
                OrgUrl = this.OrgUrl,
                //public string VenueContact { get; set; }
                VenueContact = this.VenueContact,
                //public string VenueName { get; set; }
                VenueName = this.VenueName,
                //public ICollection<Attendance> Attendees { get; set; }
                Attendees = new List<Attendance>(),
                //public ICollection<TagEvent> TagEvents { get; set; }
                EventLoops = new List<EventLoop>()
            };

            return eventHeader;
        }
    }
}