using InTheLoopAPI.Models.Database;
using InTheLoopAPI.Models.Request;
using InTheLoopAPI.Models.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InTheLoopAPI.Models
{
    public class EventHeader
    {
        public int Id { get; set; }

        public int EventFooterId { get; set; }
        public virtual EventFooter EventFooter { get; set; }

        public bool Archived { get; set; }

        public string Street { get; set; }

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

        public EventModel ToEventModel(string userId)
        {
            var model = new EventModel
            {
                Active = this.Archived,
                AgeGroup = this.EventFooter.AgeGroup,
                EventFooterId = this.EventFooterId,
                City = this.City,
                Description = this.EventFooter.Description,
                End = this.End,
                Id = this.Id,
                Latitude = this.Latitude,
                EventImageURL = this.ImageURL,
                Longitude = this.Longitude,
                Loops = this.Loops,
                Start = this.Start,
                State = this.State,
                Street = this.Street,
                Title = this.EventFooter.Title,
                Website = this.EventFooter.Website,
                ZipCode = this.ZipCode,
                Price = this.Price,
                Views = this.Views,
                Published = this.Published,
                Featured = this.Featured,
                TicketUrl = this.TicketUrl,
                OrgUrl = this.OrgUrl,
                OrgContact = this.OrgContact,
                OrgName = this.OrgName,
                VenueContact = this.VenueContact,
                VenueName = this.VenueName,
                UserId = this.EventFooter.UserId,
                IsAttending = String.IsNullOrEmpty(userId) ? false : this.Attendees.Any(u => u.UserId == userId),
                User = this.EventFooter.User == null ? null : new UserModel
                {
                    FollowersCount = this.EventFooter.User.Followers.Count,
                    UserName = this.EventFooter.User.UserName,
                    Loops = this.EventFooter.User.AttendEvents.Count,
                    UserId = this.EventFooter.UserId,
                    ImageURL = this.EventFooter.User.ImageURL
                },
                Tags = new List<TagModel>()
            };

            foreach(TagEvent tagEvent in this.TagEvents)
            {
                model.Tags.Add(new TagModel { TagId = tagEvent.Tag.Id, TagName = tagEvent.Tag.Name });
            }

            return model;
        }
    }
}