using InTheLoopAPI.Models;
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
            _eventRepository = new EventRepository();
            _validator = new EventValidator();
            _repository = new DatabaseContext();
        }

        public List<ValidationResult> AddEvent(string userId, EventModel eventModel)
        {
            EventFooter baseEvent = new EventFooter
            {
                AgeGroup = eventModel.AgeGroup,
                Category = eventModel.Category,
                Description = eventModel.Description,
                Logo = eventModel.Logo,
                Title = eventModel.Title,
                Website = eventModel.Website
            };

            EventHeader eventt = new EventHeader
            {
                City = eventModel.City,
                End = eventModel.End,
                Latitude = eventModel.Latitude,
                Longitude = eventModel.Longitude,
                Start = eventModel.Start,
                State = eventModel.State,
                UserId = userId,
                ZipCode = eventModel.ZipCode
            };

            var baseEventResults = _validator.IsValid(baseEvent).ToList();

            var eventResults = _validator.IsValid(eventt).ToList();

            var results = baseEventResults.Union(eventResults);

            if (results.Any())
                return results.ToList();

            eventt.BaseEvent = baseEvent;

            _repository.Events.Add(eventt);
            _repository.SaveChanges();

            return results.ToList();
        }

        public List<ValidationResult> AddEventHeader(string userId, EventHeaderModel model)
        {
            var repeatEvent = new EventHeader
            {
                BaseEventId = model.BaseEventId,
                City = model.City,
                End = model.End,
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                Start = model.Start,
                State = model.State,
                UserId = userId,
                ZipCode = model.ZipCode
            };

            var results = _validator.IsValid(repeatEvent);

            if (results.Any())
                return results.ToList();

            _repository.Events.Add(repeatEvent);
            _repository.SaveChanges();

            return results.ToList();
        }

        public List<EventModel> GetEvents(double latitude, double longitude, double radius)
        {
            return _eventRepository.GetEvents(latitude, longitude, radius);
        }

        public EventModel GetEvent(int eventId)
        {
            return _eventRepository.GetEvent(eventId);
        }

        public ValidationResult ArchiveEvent(int eventId)
        {
            throw new NotImplementedException();
        }

        public List<ValidationResult> UpdateEvent(EventModel eventModel, int eventId)
        {
            throw new NotImplementedException();
        }
    }
}