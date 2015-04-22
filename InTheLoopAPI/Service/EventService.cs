using InTheLoopAPI.Models.RequestModels;
using InTheLoopAPI.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Service
{
    public class EventService : IEventService
    {
        public List<ValidationResult> AddNewEvent(string userId, EventModel model)
        {
            throw new NotImplementedException();
        }

        public List<ValidationResult> AddRepeatEvent(string userId, RepeatEventModel model)
        {
            throw new NotImplementedException();
        }

        public List<EventModel> GetEvents(double latitude, double longitude, int radius)
        {
            throw new NotImplementedException();
        }

        public EventModel GetEvent(int eventId)
        {
            throw new NotImplementedException();
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