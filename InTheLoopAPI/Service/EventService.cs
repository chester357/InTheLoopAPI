using InTheLoopAPI.Models;
using InTheLoopAPI.Models.Database;
using InTheLoopAPI.Models.Request;
using InTheLoopAPI.Models.RequestModels;
using InTheLoopAPI.Queries;
using InTheLoopAPI.Service.Interfaces;
using InTheLoopAPI.Service.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Service
{
    public class EventService 
    {
        private EventRepository _eventRepository;
        private EventValidator _validator;
        private DatabaseContext _repository;

        public EventService()
        {
            _repository = new DatabaseContext();
            _eventRepository = new EventRepository(_repository);
            _validator = new EventValidator(_eventRepository);
        }

        public ValidationResult PublishEvent(EventModel eventModel, string userId)
        {
            if(eventModel == null || eventModel.Id == 0)
            {
                return new ValidationResult("No Event Found");
            }

            var eventHeader = _repository.EventHeaders.Single(x => x.Id == eventModel.Id);

            if(eventHeader.EventFooter.UserId != userId)
            {
                return new ValidationResult("Not your event, Asshole");
            }

            eventHeader.Published = true;

            _repository.SaveChanges();

            return ValidationResult.Success;
        }

        public List<EventModel> GetMyPublishedEvents(string userId)
        {
            return _eventRepository.GetMyPublishedEvents(userId);
        }

        public List<EventModel> GetPartialEvents(string userId)
        {
            return _eventRepository.GetPartialEventsForUser(userId);
        }

        public ValidationResult DeletePartialEvent(EventModel eventModel)
        {
            if(eventModel != null && eventModel.Id != 0)
            {
                var eventHeader = _repository.EventHeaders.SingleOrDefault(x => x.Id == eventModel.Id);

                var result = _repository.EventHeaders.Remove(eventHeader);

                _repository.SaveChanges();

                if (result != null)
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult("Event not removed");
        }

        public EventModel AddPartialEvent(string userId, EventModel eventModel)
        {
            // THIS IS A BRAND NEW EVENT, CREATE A NEW EVENT HEADER AND FOOTER

            var eventHeader = eventModel.ToEventHeader(userId);

            if (eventModel.Tags != null)
            {
                foreach (TagModel t in eventModel.Tags)
                {
                    t.TagName = t.TagName.Trim();

                    var tag = _repository.Tags.SingleOrDefault(x => x.Name.ToLower() == t.TagName.ToLower());

                    if (tag == null)
                    {
                        tag = new Tag { Name = t.TagName };

                        eventHeader.TagEvents.Add(new TagEvent { Tag = tag });
                    }
                    else
                        eventHeader.TagEvents.Add(new TagEvent { Tag = tag });
                }
            }

            _repository.EventHeaders.Add(eventHeader);
            _repository.SaveChanges();

            var model = eventHeader.ToEventModel(userId);

            return model;
        }

        public EventModel AddOrUpdatePartialEvent(string userId, EventModel eventModel)
        {

            var eventHeader = eventModel.ToEventHeader(userId);

            if (eventHeader.Id <= 0 || eventHeader.EventFooterId <= 0)
            {
                return AddPartialEvent(userId, eventModel);
            }

            var existingEventHeader = _repository.EventHeaders.Any(x => x.Id == eventModel.Id);

            if (!existingEventHeader)
                return null;

            foreach (TagModel t in eventModel.Tags)
            {
                var tag = _repository.Tags.SingleOrDefault(x => x.Name == t.TagName);

                if (tag == null)
                {
                    tag = new Tag { Name = t.TagName };

                    eventHeader.TagEvents.Add(new TagEvent { Tag = tag });
                }
                else
                    eventHeader.TagEvents.Add(new TagEvent { Tag = tag });
            }

            // UPDATE EVENT HEADER AND FOOTER

            _repository.SaveChanges();

            var model = eventHeader.ToEventModel(userId);

            return model;
        }

        public List<ValidationResult> AddEvent(string userId, EventModel eventModel)
        {
            if(!eventModel.Website.Contains("http://") && !eventModel.Website.Contains("https://"))
                eventModel.Website = "http://" + eventModel.Website;

            EventFooter eventFooter = new EventFooter
            {
                AgeGroup = eventModel.AgeGroup,
                Description = eventModel.Description,
                Title = eventModel.Title,
                UserId = userId,
                Website = eventModel.Website
            };

            EventHeader eventHeader = new EventHeader
            {
                City = eventModel.City,
                End = eventModel.End,
                Latitude = eventModel.Latitude,
                Longitude = eventModel.Longitude,
                Start = eventModel.Start,
                State = eventModel.State,
                ZipCode = eventModel.ZipCode,
                Price = eventModel.Price,
                ImageURL = eventModel.EventImageURL,
                Archived = false,
                Views = 0,
                TagEvents = new List<TagEvent>()
            };

            foreach(TagModel t in eventModel.Tags)
            {
                t.TagName = t.TagName.Trim();

                var tag = _repository.Tags.SingleOrDefault(x => x.Name.ToLower() == t.TagName.ToLower());

                if(tag == null)
                {
                    tag = new Tag { Name = t.TagName };

                    eventHeader.TagEvents.Add(new TagEvent { Tag = tag });
                }
                else
                    eventHeader.TagEvents.Add(new TagEvent { TagId = tag.Id });
            }

            var footerResults = _validator.EventFooter(eventFooter).ToList();

            var headerResults = _validator.EventHeader(eventHeader, userId).ToList();

            var results = footerResults.Union(headerResults);

            if (results.Any())
                return results.ToList();

            eventHeader.EventFooter = eventFooter;

            _repository.EventHeaders.Add(eventHeader);
            _repository.SaveChanges();

            return results.ToList();
        }

        public List<ValidationResult> AddEventHeader(string userId, EventHeaderModel model)
        {
            var repeatEvent = new EventHeader
            {
                EventFooterId = model.EventFooterId,
                City = model.City,
                End = model.End,
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                Start = model.Start,
                State = model.State,
                ZipCode = model.ZipCode,
                TagEvents = new List<TagEvent>()
            };

            foreach (TagModel t in model.Tags)
            {
                t.TagName = t.TagName.Trim();

                var tag = _repository.Tags.SingleOrDefault(x => x.Name.ToLower() == t.TagName.ToLower());

                if (tag == null)
                {
                    tag = new Tag { Name = t.TagName };

                    repeatEvent.TagEvents.Add(new TagEvent { Tag = tag });
                }
                else
                    repeatEvent.TagEvents.Add(new TagEvent { TagId = tag.Id });
            }

            var results = _validator.EventHeader(repeatEvent, userId);

            if (results.Any())
                return results.ToList();

            _repository.EventHeaders.Add(repeatEvent);
            _repository.SaveChanges();

            return results.ToList();
        }

        public List<EventModel> GetEvents(string userId, double latitude, double longitude, double radius)
        {
            return _eventRepository.GetEvents(userId, latitude, longitude, radius);
        }

        public List<EventModel> GetUserEvents(string userId)
        {
            return _eventRepository.GetUserEvents(userId, false);
        }

        public List<EventModel> GetHomeEvents(string userId, double latitude, double longitude, double radius)
        {
            return _eventRepository.GetHomeEvents(userId, latitude, longitude, radius);
        }

        public List<EventModel> GetMostPopularToday(string userId, double latitude, double longitude, double radius, DateTime today)
        {
            return _eventRepository.GetMostPopularToday(userId, latitude, longitude, radius, today);
        }

        public List<EventModel> GetMostPopularThisWeekend(string userId, double latitude, double longitude, double radius, DateTime today)
        {
            return _eventRepository.GetMostPopularThisWeekend(userId, latitude, longitude, radius, today);
        }

        public EventModel GetEvent(string userId, int eventId)
        {            
            return _eventRepository.GetEvent(eventId, userId);
        }

        public ValidationResult UpdateViewCount(int eventId)
        {
            var evt =_repository.EventHeaders.SingleOrDefault(x => x.Id == eventId);

            if(evt == null) { return new ValidationResult("No event found"); }

            evt.Views++;

            _repository.SaveChanges();

            return ValidationResult.Success;
        }       

        public ValidationResult ArchiveEvent(int eventHeaderId, string userId)
        {
            var result = _validator.ArchiveEvent(eventHeaderId, userId);

            if (result != ValidationResult.Success)
                return result;

            var eventheader = _eventRepository.GetEventHeader(eventHeaderId, userId);

            eventheader.Archived = !eventheader.Archived;

            _repository.SaveChanges();

            return ValidationResult.Success;
        }

        public List<ValidationResult> UpdateEventHeader(EventHeaderModel eventHeaderModel, string userId)
        {
            var eventHeader = _eventRepository.GetEventHeader(eventHeaderModel.Id, userId);

            if (eventHeader == null)
                return new List<ValidationResult> { new ValidationResult("Invalid Event Header Id.") };

            eventHeader.City = eventHeaderModel.City;
            eventHeader.Latitude = eventHeaderModel.Latitude;
            eventHeader.Longitude = eventHeaderModel.Longitude;
            eventHeader.End = eventHeaderModel.End;
            eventHeader.ImageURL = eventHeaderModel.EventImageURL;
            eventHeader.Start = eventHeaderModel.Start;
            eventHeader.State = eventHeaderModel.State;
            eventHeader.ZipCode = eventHeaderModel.ZipCode;
            eventHeader.TagEvents = new List<TagEvent>();

            foreach (TagModel t in eventHeaderModel.Tags)
            {
                var tag = _repository.Tags.SingleOrDefault(x => x.Name == t.TagName);

                if (tag == null)
                {
                    tag = new Tag { Name = t.TagName };

                    eventHeader.TagEvents.Add(new TagEvent { Tag = tag });
                }
                else
                    eventHeader.TagEvents.Add(new TagEvent { TagId = tag.Id });
            }

            var results = _validator.EventHeader(eventHeader, userId);

            if (results.Any())
                return results.ToList();

            _repository.SaveChanges();

            return results.ToList();
        }

        public List<ValidationResult> UpdateEventFooter(EventFooterModel footerModel, string userId)
        {
            var eventFooter = _eventRepository.GetEventFooter(footerModel.Id, userId);

            if (eventFooter == null)
                return new List<ValidationResult> { new ValidationResult("Invalid Event Footer Id.") };

            eventFooter.AgeGroup = footerModel.AgeGroup;
            eventFooter.Description = footerModel.Description;
            eventFooter.Title = footerModel.Title;
            eventFooter.Website = footerModel.Website;

            var results = _validator.EventFooter(eventFooter);

            if (results.Any())
                return results.ToList();

            _repository.SaveChanges();

            return results.ToList();
        }
    }
}