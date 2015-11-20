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

        public List<TagModel> GetTags(String userId)
        {
            return _repository.Tags.Where(x => x.TagUsers.Any(t => t.UserId == userId))
                .Select(tm => new TagModel
                {
                    TagName = tm.Name,
                    TagId = tm.Id
                })
                .ToList();
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
                var tag = _repository.Tags.SingleOrDefault(x => x.Name == t.TagName);

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
                var tag = _repository.Tags.SingleOrDefault(x => x.Name == t.TagName);

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

        public List<EventModel> GetEvents(double latitude, double longitude, double radius)
        {
            return _eventRepository.GetEvents(latitude, longitude, radius);
        }

        public List<EventModel> GetHomeEvents(string userId)
        {
            return _eventRepository.GetHomeEvents(userId);
        }

        public EventModel GetEvent(int eventId)
        {
            return _eventRepository.GetEvent(eventId);
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