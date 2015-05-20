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
            _repository = new DatabaseContext();
            _eventRepository = new EventRepository(_repository);
            _validator = new EventValidator(_eventRepository);
        }

        public List<ValidationResult> AddEvent(string userId, EventModel eventModel)
        {
            EventFooter eventFooter = new EventFooter
            {
                AgeGroup = eventModel.AgeGroup,
                Category = eventModel.Category,
                Description = eventModel.Description,
                Logo = eventModel.Logo,
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
                ZipCode = eventModel.ZipCode
            };

            var footerResults = _validator.EventFooter(eventFooter).ToList();

            var headerResults = _validator.EventHeader(eventHeader, userId).ToList();

            var results = footerResults.Union(headerResults);

            if (results.Any())
                return results.ToList();

            eventHeader.BaseEvent = eventFooter;

            _repository.EventHeaders.Add(eventHeader);
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
                ZipCode = model.ZipCode
            };

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

        public List<ValidationResult> UpdateEvent(EventModel eventModel, int eventId)
        {
            throw new NotImplementedException();
        }
    }
}