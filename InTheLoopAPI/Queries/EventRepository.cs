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
                .Include("EventLoops")
                .Include("Attendees")
                .Where(
                    x => x.Published == false &&
                    x.Archived == false &&
                    x.EventFooter.UserId == userId)
                .OrderByDescending(o => o.Start)
                .ToList();

            return ToEventModelList(events, userId);
        }

        public EventModel GetEvent(int eventId, string userId)
        {
            var singleEvent = EventHeaders.Include("EventLoops").SingleOrDefault(x => 
            x.Id == eventId &&
            x.Archived == false);
            
            if (singleEvent == null)
                return null;

            singleEvent.Views++;

            Database.SaveChanges();

            return new EventModel
            {
                Active = singleEvent.Archived,
                EventFooterId = singleEvent.EventFooterId,
                City = singleEvent.City,
                Description = singleEvent.EventFooter.Description,
                End = singleEvent.End,
                Id = singleEvent.Id,
                Latitude = singleEvent.Latitude,
                EventImageURL = singleEvent.ImageURL,
                Longitude = singleEvent.Longitude,
                Rsvps = singleEvent.Rsvps,
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
                    Rsvps = singleEvent.EventFooter.User.AttendEvents.Count,
                    UserId = singleEvent.EventFooter.UserId,
                    ImageURL = singleEvent.EventFooter.User.ImageURL
                },
                Loops = singleEvent.EventLoops.Select(t => new LoopModel
                {
                    LoopId = t.LoopId,
                    LoopName = t.Loop.Name
                }).ToList()
            };
        }

        public List<EventModel> GetMyPublishedEvents(string userId)
        {
            var eventHeaders = EventHeaders
                .Include("EventLoops")
                .Include("Attendees")
                .Where(x => x.EventFooter.UserId == userId && x.Published == true && x.Archived == false)
                .OrderByDescending(o => o.Start)
                .ToList();

            return ToEventModelList(eventHeaders, userId);
        }

        public List<EventModel> GetHomeEvents(String userId, double latitude, double longitude, double radius)
        {
            double degrees = radius / 69;
            double maxLat = latitude + degrees;
            double minLat = latitude - degrees;
            double maxLong = longitude + degrees;
            double minLong = longitude - degrees;

            var tags = Loops.Where(x => x.UserLoops.Any(u => u.UserId == userId)).Select(y => y.Id).ToList();

            var eventHeaders = EventHeaders
                .Include("EventLoops")
                .Include("Attendees")
                .Where(x =>
                    (x.Archived == false && (x.End.CompareTo(DateTime.UtcNow) >= 0)) &&
                    (
                        // All events that I'm attending
                        x.Attendees.Any(n => n.UserId == userId) ||
                        // All of my events I posted
                        x.EventFooter.UserId == userId ||
                        // All events for the tags I follow
                        (
                            x.EventLoops.Any(tagEvent => tags.Any(myTag => myTag == tagEvent.LoopId)) &&
                            x.Latitude > minLat &&
                            x.Latitude < maxLat &&
                            x.Longitude > minLong &&
                            x.Longitude < maxLong
                        ) ||

                        // All events for people I follow (their posted events)
                        x.EventFooter.User.Followers.Any(f => f.FollowingMeId == userId) ||
                        // All events for people I follow (their attended events)
                        x.Attendees.Any(a => a.User.Followers.Any(f => f.FollowingMeId == userId))
                    )
                )
                .OrderByDescending(o => o.Start)
                .ToList();

            return ToEventModelList(eventHeaders, userId);
        }

        public List<EventModel> GetMostPopularToday(String userId, double latitude, double longitude, double radius, DateTime today)
        {
            double degrees = radius / 69;
            double maxLat = latitude + degrees;
            double minLat = latitude - degrees;
            double maxLong = longitude + degrees;
            double minLong = longitude - degrees;

            var tags = Loops.Where(x => x.UserLoops.Any(u => u.UserId == userId)).Select(y => y.Id).ToList();

            var eventHeaders = EventHeaders
                .Include("EventLoops")
                .Include("Attendees")
                .Where(x =>
                    (x.Archived == false && (x.Start.Year == today.Year && x.Start.Month == today.Month && x.Start.Day == today.Day)) &&
                    (
                        // All events that I'm attending
                        x.Attendees.Any(n => n.UserId == userId) ||
                        // All of my events I posted
                        x.EventFooter.UserId == userId ||
                        // All events for the tags I follow
                        (
                            x.EventLoops.Any(tagEvent => tags.Any(myTag => myTag == tagEvent.LoopId)) &&
                            x.Latitude > minLat &&
                            x.Latitude < maxLat &&
                            x.Longitude > minLong &&
                            x.Longitude < maxLong
                        ) ||

                        // All events for people I follow (their posted events)
                        x.EventFooter.User.Followers.Any(f => f.FollowingMeId == userId) ||
                        // All events for people I follow (their attended events)
                        x.Attendees.Any(a => a.User.Followers.Any(f => f.FollowingMeId == userId))
                    )
                )
                .OrderBy(x => x.Views)
                .ToList();

            return ToEventModelList(eventHeaders, userId);
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

            var tags = Loops.Where(x => x.UserLoops.Any(u => u.UserId == userId)).Select(y => y.Id).ToList();

            var eventHeaders = EventHeaders
                .Include("EventLoops")
                .Include("Attendees")
                .Where(x =>
                    (x.Archived == false && start.CompareTo(DateTime.UtcNow) >= 0 && end.CompareTo(DateTime.UtcNow) < 0) &&
                    (
                        // All events that I'm attending
                        x.Attendees.Any(n => n.UserId == userId) ||
                        // All of my events I posted
                        x.EventFooter.UserId == userId ||
                        // All events for the tags I follow
                        (
                            x.EventLoops.Any(tagEvent => tags.Any(myTag => myTag == tagEvent.LoopId)) &&
                            x.Latitude > minLat &&
                            x.Latitude < maxLat &&
                            x.Longitude > minLong &&
                            x.Longitude < maxLong
                        ) ||

                        // All events for people I follow (their posted events)
                        x.EventFooter.User.Followers.Any(f => f.FollowingMeId == userId) ||
                        // All events for people I follow (their attended events)
                        x.Attendees.Any(a => a.User.Followers.Any(f => f.FollowingMeId == userId))
                    )
                )
                .OrderBy(x => x.Views)
                .ToList();

            return ToEventModelList(eventHeaders, userId);
        }

        public List<EventModel> GetEvents(string userId, double latitude, double longitude, double radius, FilterModel filter)
        {
            if(filter.Costs == null)
            {
                filter.Costs = new List<int>();
            }

            if (filter.Tags == null)
            {
                filter.Tags = new List<string>();
            }

            double degrees = radius / 69;
            double maxLat = latitude + degrees;
            double minLat = latitude - degrees;
            double maxLong = longitude + degrees;
            double minLong = longitude - degrees;

            var eventHeaders = new List<EventHeader>();

            if (filter.AllTags && filter.AllCosts)
            {
                eventHeaders = EventHeaders
                .Include("EventLoops")
                .Include("Attendees")
                .Where(x =>
                    x.Latitude > minLat &&
                    x.Latitude < maxLat &&
                    x.Longitude > minLong &&
                    x.Longitude < maxLong &&
                    x.Archived == false &&
                    x.Published == true &&
                    x.End.CompareTo(filter.End) <= 0 &&
                    x.Start.CompareTo(filter.Start) >= 0 
                )
                .OrderByDescending(o => o.Start)
                .ToList();
            }

            else if(filter.AllTags && !filter.AllCosts)
            {
                eventHeaders = EventHeaders
                .Include("EventLoops")
                .Include("Attendees")
                .Where(x =>
                    x.Latitude > minLat &&
                    x.Latitude < maxLat &&
                    x.Longitude > minLong &&
                    x.Longitude < maxLong &&
                    x.Archived == false &&
                    x.Published == true &&
                    x.End.CompareTo(filter.End) <= 0 &&
                    x.Start.CompareTo(filter.Start) >= 0 &&
                    filter.Costs.Contains(x.Price)
                )
                .OrderByDescending(o => o.Start)
                .ToList();
            }

            else if(!filter.AllTags && filter.AllCosts)
            {
                eventHeaders = EventHeaders
                .Include("EventLoops")
                .Include("Attendees")
                .Where(x =>
                    x.Latitude > minLat &&
                    x.Latitude < maxLat &&
                    x.Longitude > minLong &&
                    x.Longitude < maxLong &&
                    x.Archived == false &&
                    x.Published == true &&
                    x.End.CompareTo(filter.End) <= 0 &&
                    x.Start.CompareTo(filter.Start) >= 0 &&
                    x.EventLoops.Any(t => filter.Tags.Contains(t.Loop.Name.ToLower()))
                )
                .OrderByDescending(o => o.Start)
                .ToList();
            }
            else
            {
                eventHeaders = EventHeaders
                .Include("EventLoops")
                .Include("Attendees")
                .Where(x =>
                    x.Latitude > minLat &&
                    x.Latitude < maxLat &&
                    x.Longitude > minLong &&
                    x.Longitude < maxLong &&
                    x.Archived == false &&
                    x.Published == true &&
                    x.End.CompareTo(filter.End) <= 0 &&
                    x.Start.CompareTo(filter.Start) >= 0 &&
                    x.EventLoops.Any(t => filter.Tags.Contains(t.Loop.Name.ToLower())) &&
                    filter.Costs.Contains(x.Price)
                )
                .OrderByDescending(o => o.Start)
                .ToList();
            }

            return ToEventModelList(eventHeaders, userId);
        }

        public List<EventModel> GetUserEvents(string userId, bool privateAccount)
        {
            if (!privateAccount)
            {
                var eventHeaders = EventHeaders
                .Include("EventLoops")
                .Include("Attendees")
                .Where(x =>
                     (x.Archived == false && x.Published && (x.End.CompareTo(DateTime.UtcNow) >= 0)) &&
                    (
                        // All events that I'm attending
                        x.Attendees.Any(n => n.UserId == userId) ||
                        // All of my events I posted
                        x.EventFooter.UserId == userId
                    )
                )
                .OrderByDescending(o => o.Start)
                .ToList();

                return ToEventModelList(eventHeaders, userId);
            }
            else
            {
                var eventHeaders = EventHeaders
                .Include("EventLoops")
                .Include("Attendees")
                .Where(x =>
                     (x.Archived == false && x.Published && (x.End.CompareTo(DateTime.UtcNow) >= 0)) &&
                    (
                        // All of my events I posted
                        x.EventFooter.UserId == userId
                    )
                )
                .OrderByDescending(o => o.Start)
                .ToList();

                return ToEventModelList(eventHeaders, userId);
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

        public List<EventModel> ToEventModelList(List<EventHeader> eventHeaders, string userId)
        {
            var modelList = new List<EventModel>();

            foreach (var _event in eventHeaders )
            {
                modelList.Add(_event.ToEventModel(userId));
            }

            return modelList;
        }
    }
}