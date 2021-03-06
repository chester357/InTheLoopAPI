﻿using InTheLoopAPI.Models;
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

        public EventModel GetEvent(int eventId)
        {
            var singleEvent = EventHeaders.SingleOrDefault(x => x.Id == eventId);

            if (singleEvent == null)
                return null;

            return new EventModel
            {
                Active = singleEvent.Archived,
                AgeGroup = singleEvent.EventFooter.AgeGroup,
                EventFooterId = singleEvent.EventFooterId,
                Category = singleEvent.EventFooter.Category,
                City = singleEvent.City,
                Description = singleEvent.EventFooter.Description,
                End = singleEvent.End,
                Id = singleEvent.Id,
                Latitude = singleEvent.Latitude,
                Logo = singleEvent.EventFooter.Logo,
                Longitude = singleEvent.Longitude,
                Loops = singleEvent.Loops,
                Start = singleEvent.Start,
                State = singleEvent.State,
                Title = singleEvent.EventFooter.Title,
                Website = singleEvent.EventFooter.Website,
                ZipCode = singleEvent.ZipCode,
                Price = singleEvent.Price
            };
        }
    
        public List<EventModel> GetEvents(double latitude, double longitude, double radius)
        {
            double degrees = radius / 69;
            double maxLat = latitude + degrees;
            double minLat = latitude - degrees;
            double maxLong = longitude + degrees;
            double minLong = longitude - degrees;

            return EventHeaders
                .Where(x => x.Latitude > minLat && x.Latitude < maxLat && x.Longitude > minLong && x.Longitude < maxLong)
                .Select(y => new EventModel
                {
                    Active = y.Archived,
                    AgeGroup = y.EventFooter.AgeGroup,
                    EventFooterId = y.EventFooterId,
                    Category = y.EventFooter.Category,
                    City = y.City,
                    Description = y.EventFooter.Description,
                    End = y.End,
                    Id = y.Id,
                    Latitude = y.Latitude,
                    Logo = y.EventFooter.Logo,
                    Longitude = y.Longitude,
                    Loops = y.Loops,
                    Start = y.Start,
                    State = y.State,
                    Title = y.EventFooter.Title,
                    Website = y.EventFooter.Website,
                    ZipCode = y.ZipCode,
                    Price = y.Price
                })
                .ToList();
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