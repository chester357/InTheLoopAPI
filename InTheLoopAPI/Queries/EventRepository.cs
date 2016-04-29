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

        public List<EventModel> GetPartialEventsForUser(string userId)
        {
            var events = EventHeaders
                .Where(
                    x => x.Published == false &&
                    x.Archived == false &&
                    x.EventFooter.UserId == userId)
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

                    Published = y.Published,
                    Featured = y.Featured,
                    TicketUrl = y.TicketUrl,
                    OrgUrl = y.OrgUrl,
                    OrgContact = y.OrgContact,
                    OrgName = y.OrgName,
                    VenueContact = y.VenueContact,
                    VenueName = y.VenueName,

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

            return events;
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

        public List<EventModel> GetMyPublishedEvents(string userId)
        {

            return EventHeaders.Where(x => x.EventFooter.UserId == userId && x.Published == true)
                .OrderByDescending(o => o.Start)
                .Select(s => s.ToEventModel(userId))
                .ToList();
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

                    Published = y.Published,
                    Featured = y.Featured,
                    TicketUrl = y.TicketUrl,
                    OrgUrl = y.OrgUrl,
                    OrgContact = y.OrgContact,
                    OrgName = y.OrgName,
                    VenueContact = y.VenueContact,
                    VenueName = y.VenueName,

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

        public List<EventModel> GetMostPopularToday(String userId, double latitude, double longitude, double radius, DateTime today)
        {
            double degrees = radius / 69;
            double maxLat = latitude + degrees;
            double minLat = latitude - degrees;
            double maxLong = longitude + degrees;
            double minLong = longitude - degrees;

            var tags = Tags.Where(x => x.TagUsers.Any(u => u.UserId == userId)).Select(y => y.Id).ToList();

            return EventHeaders
                .Where(x =>
                    (x.Archived == false && (x.Start.Year == today.Year && x.Start.Month == today.Month && x.Start.Day == today.Day)) &&
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
                .OrderBy(x => x.Views)
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
                    Published = y.Published,
                    Featured = y.Featured,
                    TicketUrl = y.TicketUrl,
                    OrgUrl = y.OrgUrl,
                    OrgContact = y.OrgContact,
                    OrgName = y.OrgName,
                    VenueContact = y.VenueContact,
                    VenueName = y.VenueName,
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

        public List<EventModel> GetMostPopularThisWeekend(String userId, double latitude, double longitude, double radius, DateTime today)
        {
            double degrees = radius / 69;
            double maxLat = latitude + degrees;
            double minLat = latitude - degrees;
            double maxLong = longitude + degrees;
            double minLong = longitude - degrees;

            var start = DateTime.UtcNow;
            var end = DateTime.UtcNow;

            switch (today.DayOfWeek) {
                case DayOfWeek.Monday:
                    //daysToFriday = 4;
                    start = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day).AddDays(4);
                    //daysToSunday = 6;
                    end = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day).AddHours(23).AddMinutes(59).AddDays(6);
                    break;
                case DayOfWeek.Tuesday:
                    //daysToFriday = 3;
                    start = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day).AddDays(3);
                    //daysToSunday = 5;
                    end = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day).AddHours(23).AddMinutes(59).AddDays(5);
                    break;
                case DayOfWeek.Wednesday:
                    //daysToFriday = 2;
                    start = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day).AddDays(2);
                    //daysToSunday = 4;
                    end = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day).AddHours(23).AddMinutes(59).AddDays(4);
                    break;
                case DayOfWeek.Thursday:
                    //daysToFriday = 1;
                    start = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day).AddDays(1);
                    //daysToSunday = 3;
                    end = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day).AddHours(23).AddMinutes(59).AddDays(3);
                    break;
                case DayOfWeek.Friday:
                    //daysToFriday = 0;
                    start = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day).AddDays(0);
                    //daysToSunday = 2;
                    end = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day).AddHours(23).AddMinutes(59).AddDays(2);
                    break;
                case DayOfWeek.Saturday:
                    //daysToFriday = 6;
                    start = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day).AddDays(0);
                    //daysToSunday = 1;
                    end = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day).AddHours(23).AddMinutes(59).AddDays(1);
                    break;
                case DayOfWeek.Sunday:
                    //daysToFriday = 5;
                    start = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day).AddDays(0);
                    //daysToSunday = 0;
                    end = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day).AddHours(23).AddMinutes(59).AddDays(0);
                    break;
            }

            var tags = Tags.Where(x => x.TagUsers.Any(u => u.UserId == userId)).Select(y => y.Id).ToList();

            return EventHeaders
                .Where(x =>
                    (x.Archived == false && start.CompareTo(DateTime.UtcNow) >= 0 && end.CompareTo(DateTime.UtcNow) < 0) &&
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
                .OrderBy(x => x.Views)
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
                    Published = y.Published,
                    Featured = y.Featured,
                    TicketUrl = y.TicketUrl,
                    OrgUrl = y.OrgUrl,
                    OrgContact = y.OrgContact,
                    OrgName = y.OrgName,
                    VenueContact = y.VenueContact,
                    VenueName = y.VenueName,
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
                    Published = y.Published,
                    Featured = y.Featured,
                    TicketUrl = y.TicketUrl,
                    OrgUrl = y.OrgUrl,
                    OrgContact = y.OrgContact,
                    OrgName = y.OrgName,
                    VenueContact = y.VenueContact,
                    VenueName = y.VenueName,
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
                    Published = y.Published,
                    Featured = y.Featured,
                    TicketUrl = y.TicketUrl,
                    OrgUrl = y.OrgUrl,
                    OrgContact = y.OrgContact,
                    OrgName = y.OrgName,
                    VenueContact = y.VenueContact,
                    VenueName = y.VenueName,
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