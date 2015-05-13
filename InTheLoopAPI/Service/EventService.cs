using InTheLoopAPI.Models.RequestModels;
using InTheLoopAPI.Queries;
using InTheLoopAPI.Service.Interfaces;
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

        public EventService()
        {
            _eventRepository = new EventRepository();
        }

        public List<ValidationResult> AddNewEvent(string userId, EventModel model)
        {
            throw new NotImplementedException();
        }

        public List<ValidationResult> AddRepeatEvent(string userId, RepeatEventModel model)
        {
            throw new NotImplementedException();
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