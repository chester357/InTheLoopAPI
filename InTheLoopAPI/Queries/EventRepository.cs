using InTheLoopAPI.Models;
using InTheLoopAPI.Models.Request;
using InTheLoopAPI.Models.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Queries
{
    public class EventRepository : BaseQuery
    {
        public EventRepository(DatabaseContext db) : base(db)
        {

        }

        public EventModel GetEvent(int eventId, string userId)
        {
            var singleEvent = EventHeaders.Include("TagEvents").SingleOrDefault(x => 
            x.Id == eventId &&
            x.Archived == false);
            
            if (singleEvent == null)
                return null;

            singleEvent.Views++;

            Database.SaveChanges();

            return new EventModel
            {
                Active = singleEvent.Archived,
                AgeGroup = singleEvent.EventFooter.AgeGroup,
                EventFooterId = singleEvent.EventFooterId,
                City = singleEvent.City,
                Description = singleEvent.EventFooter.Description,
                End = singleEvent.End,
                Id = singleEvent.Id,
                Latitude = singleEvent.Latitude,
                EventImageURL = singleEvent.ImageURL,
                Longitude = singleEvent.Longitude,
                Loops = singleEvent.Loops,
                Start = singleEvent.Start,
                State = singleEvent.State,
                Title = singleEvent.EventFooter.Title,
                Website = singleEvent.EventFooter.Website,
                ZipCode = singleEvent.ZipCode,
                Price = singleEvent.Price,
                Views = singleEvent.Views,
                UserId = singleEvent.EventFooter.UserId,
                UserProfileURL = singleEvent.EventFooter.User.ImageURL,
                IsAttending = singleEvent.Attendees.Any(u => u.UserId == userId),
                User = new UserModel
                {
                    FollowersCount = singleEvent.EventFooter.User.Followers.Count,
                    UserName =  singleEvent.EventFooter.User.UserName,
                    Loops = singleEvent.EventFooter.User.AttendEvents.Count,
                    UserId = singleEvent.EventFooter.UserId,
                    ImageURL = singleEvent.EventFooter.User.ImageURL
                },
                Tags = singleEvent.TagEvents.Select(t => new TagModel
                {
                    TagId = t.TagId,
                    TagName = t.Tag.Name
                }).ToList()
            };
        }

        public List<EventModel> GetHomeEvents(String userId, double latitude, double longitude, double radius)
        {
            double degrees = radius / 69;
            double maxLat = latitude + degrees;
            double minLat = latitude - degrees;
            double maxLong = longitude + degrees;
            double minLong = longitude - degrees;

            var tags = Tags.Where(x => x.TagUsers.Any(u => u.UserId == userId)).Select(y => y.Id).ToList();

            return EventHeaders
                .Where(x =>
                    (x.Archived == false && (x.End.CompareTo(DateTime.UtcNow) >= 0)) &&
                    (
                        // All events that I'm attending
                        x.Attendees.Any(n => n.UserId == userId) ||
                        // All of my events I posted
                        x.EventFooter.UserId == userId ||
                        // All events for the tags I follow
                        (
                            x.TagEvents.Any(tagEvent => tags.Any(myTag => myTag == tagEvent.TagId)) &&
                            x.Latitude > minLat &&
                            x.Latitude < maxLat &&
                            x.Longitude > minLong &&
                            x.Longitude < maxLong
                        ) ||

                        // All events for people I follow (their posted events)
                        x.EventFooter.User.Followers.Any(f => f.UserId == userId) ||
                        // All events for people I follow (their attended events)
                        x.Attendees.Any(a => a.User.Followers.Any(f => f.UserId == userId))
                    )
                )               
                .OrderBy(x => x.Start)
                .Select(y => new EventModel
                {
                    Active = y.Archived,
                    AgeGroup = y.EventFooter.AgeGroup,
                    EventFooterId = y.EventFooterId,
                    City = y.City,
                    Description = y.EventFooter.Description,
                    End = y.End,
                    Id = y.Id,
                    Latitude = y.Latitude,
                    EventImageURL = y.ImageURL,
                    Longitude = y.Longitude,
                    Loops = y.Loops,
                    Start = y.Start,
                    State = y.State,
                    Title = y.EventFooter.Title,
                    Website = y.EventFooter.Website,
                    ZipCode = y.ZipCode,
                    Price = y.Price,
                    Views = y.Views,
                    UserId = y.EventFooter.UserId,
                    UserProfileURL = y.EventFooter.User.ImageURL,
                    IsAttending = y.Attendees.Any(u => u.UserId == userId),
                    User = new UserModel
                    {
                        FollowersCount = y.EventFooter.User.Followers.Count,
                        UserName = y.EventFooter.User.UserName,
                        Loops = y.EventFooter.User.AttendEvents.Count,
                        UserId = y.EventFooter.UserId,
                        ImageURL = y.EventFooter.User.ImageURL
                    },
                    Tags = y.TagEvents
                    .Select(t => new TagModel
                    {
                        TagName = t.Tag.Name,
                        TagId = t.TagId
                    })
                    .ToList()
                })
                .ToList();
        }
    
        public List<EventModel> GetEvents(string userId, double latitude, double longitude, double radius)
        {
            double degrees = radius / 69;
            double maxLat = latitude + degrees;
            double minLat = latitude - degrees;
            double maxLong = longitude + degrees;
            double minLong = longitude - degrees;

            return EventHeaders
                .Where(x => 
                    x.Latitude > minLat && 
                    x.Latitude < maxLat && 
                    x.Longitude > minLong && 
                    x.Longitude < maxLong && 
                    x.Archived == false &&
                    x.End.CompareTo(DateTime.UtcNow) >= 0)
                .Select(y => new EventModel
                {
                    Active = y.Archived,
                    AgeGroup = y.EventFooter.AgeGroup,
                    EventFooterId = y.EventFooterId,
                    City = y.City,
                    Description = y.EventFooter.Description,
                    End = y.End,
                    Id = y.Id,
                    Latitude = y.Latitude,
                    EventImageURL = y.ImageURL,
                    Longitude = y.Longitude,
                    Loops = y.Loops,
                    Start = y.Start,
                    State = y.State,
                    Title = y.EventFooter.Title,
                    Website = y.EventFooter.Website,
                    ZipCode = y.ZipCode,
                    Price = y.Price,
                    Views = y.Views,
                    UserId = y.EventFooter.UserId,
                    UserProfileURL = y.EventFooter.User.ImageURL,
                    IsAttending = y.Attendees.Any(u => u.UserId == userId),
                    User = new UserModel
                    {
                        FollowersCount = y.EventFooter.User.Followers.Count,
                        UserName = y.EventFooter.User.UserName,
                        Loops = y.EventFooter.User.AttendEvents.Count,
                        UserId = y.EventFooter.UserId,
                        ImageURL = y.EventFooter.User.ImageURL
                    },
                    Tags = y.TagEvents
                    .Select(t => new TagModel
                    {
                        TagName = t.Tag.Name,
                        TagId = t.TagId
                    })
                    .ToList()
                })
                .ToList();
        }

        public List<EventModel> GetUserEvents(string userId, bool privateAccount)
        {
            if (!privateAccount)
            {
                return EventHeaders
                .Where(x =>
                    (x.Archived == false && (x.End.CompareTo(DateTime.UtcNow) >= 0)) &&
                    (
                        // All events that I'm attending
                        x.Attendees.Any(n => n.UserId == userId) ||
                        // All of my events I posted
                        x.EventFooter.UserId == userId
                    )
                )
                .OrderBy(x => x.Start)
                .Select(y => new EventModel
                {
                    Active = y.Archived,
                    AgeGroup = y.EventFooter.AgeGroup,
                    EventFooterId = y.EventFooterId,
                    City = y.City,
                    Description = y.EventFooter.Description,
                    End = y.End,
                    Id = y.Id,
                    Latitude = y.Latitude,
                    EventImageURL = y.ImageURL,
                    Longitude = y.Longitude,
                    Loops = y.Loops,
                    Start = y.Start,
                    State = y.State,
                    Title = y.EventFooter.Title,
                    Website = y.EventFooter.Website,
                    ZipCode = y.ZipCode,
                    Price = y.Price,
                    Views = y.Views,
                    UserId = y.EventFooter.UserId,
                    UserProfileURL = y.EventFooter.User.ImageURL,
                    IsAttending = y.Attendees.Any(u => u.UserId == userId),
                    User = new UserModel
                    {
                        FollowersCount = y.EventFooter.User.Followers.Count,
                        UserName = y.EventFooter.User.UserName,
                        Loops = y.EventFooter.User.AttendEvents.Count,
                        UserId = y.EventFooter.UserId,
                        ImageURL = y.EventFooter.User.ImageURL
                    },
                    Tags = y.TagEvents
                    .Select(t => new TagModel
                    {
                        TagName = t.Tag.Name,
                        TagId = t.TagId
                    })
                    .ToList()
                })
                .ToList();
            }
            else
            {
                return EventHeaders
                .Where(x =>
                    (x.Archived == false && (x.End.CompareTo(DateTime.UtcNow) >= 0)) &&
                    (
                        // All of my events I posted
                        x.EventFooter.UserId == userId 
                    )
                )
                .OrderBy(x => x.Start)
                .Select(y => new EventModel
                {
                    Active = y.Archived,
                    AgeGroup = y.EventFooter.AgeGroup,
                    EventFooterId = y.EventFooterId,
                    City = y.City,
                    Description = y.EventFooter.Description,
                    End = y.End,
                    Id = y.Id,
                    Latitude = y.Latitude,
                    EventImageURL = y.ImageURL,
                    Longitude = y.Longitude,
                    Loops = y.Loops,
                    Start = y.Start,
                    State = y.State,
                    Title = y.EventFooter.Title,
                    Website = y.EventFooter.Website,
                    ZipCode = y.ZipCode,
                    Price = y.Price,
                    Views = y.Views,
                    UserId = y.EventFooter.UserId,
                    UserProfileURL = y.EventFooter.User.ImageURL,
                    IsAttending = y.Attendees.Any(u => u.UserId == userId),
                    User = new UserModel
                    {
                        FollowersCount = y.EventFooter.User.Followers.Count,
                        UserName = y.EventFooter.User.UserName,
                        Loops = y.EventFooter.User.AttendEvents.Count,
                        UserId = y.EventFooter.UserId,
                        ImageURL = y.EventFooter.User.ImageURL
                    },
                    Tags = y.TagEvents
                    .Select(t => new TagModel
                    {
                        TagName = t.Tag.Name,
                        TagId = t.TagId
                    })
                    .ToList()
                })
                .ToList();
            }
        }
        public bool ValidEventFooterId(int id)
        {
            return EventFooters.Any(x => x.Id == id);
        }   

        public bool ValidEventHeaderId(int id)
        {
            return EventHeaders.Any(x => x.Id == id);
        }

        public bool ValidUserForEventFooter(string userId, int eventFooterId)
        {
            return EventFooters.Any(x => x.UserId == userId && x.Id == eventFooterId);
        }

        public bool ValidUserForEventHeader(string userId, int eventHeaderId)
        {
            return EventHeaders.Any(x => x.EventFooter.UserId == userId && x.Id == eventHeaderId);
        }

        public EventHeader GetEventHeader(int eventHeaderId, string userId)
        {
            return EventHeaders.SingleOrDefault(x => x.Id == eventHeaderId && x.EventFooter.UserId == userId);
        }

        public EventFooter GetEventFooter(int eventFoooterId, string userId)
        {
            return EventFooters.SingleOrDefault(x => x.Id == eventFoooterId && x.UserId == userId);
        }
    }
}