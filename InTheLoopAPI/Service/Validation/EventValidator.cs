using InTheLoopAPI.Helpers;
using InTheLoopAPI.Models;
using InTheLoopAPI.Queries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;

namespace InTheLoopAPI.Service.Validation
{
    public class EventValidator 
    {
        private EventRepository _eventRepository;
        public DatabaseContext _databaseContext;

        public EventValidator()
        {
            _databaseContext = new DatabaseContext();

            _eventRepository = new EventRepository(_databaseContext);
        }

        public IEnumerable<ValidationResult> IsValid(EventHeader newEvent, string userId)
        {
            if (!_eventRepository.ValidUserForEvent(userId, newEvent.Id))
                yield return new ValidationResult("Invalid valid user for this event");

            if (String.IsNullOrEmpty(newEvent.City))
                yield return new ValidationResult("Invalid City.");

            if (newEvent.End.CompareTo(newEvent.Start) <= 0)
                yield return new ValidationResult("Invalid Dates.");

            if (newEvent.Latitude < -90 || newEvent.Latitude > 90)
                yield return new ValidationResult("Invalid Latitude.");

            if (newEvent.Longitude < -180 || newEvent.Longitude > 180)
                yield return new ValidationResult("Invalid Longitude.");

            if (newEvent.ZipCode.ToString().Length != 5)
                yield return new ValidationResult("Invalid Zip Code.");

            if (newEvent.BaseEventId != 0)
                if (!_eventRepository.ValidEventFooterId(newEvent.BaseEventId))
                    yield return new ValidationResult("Invalid Base Event Id.");
        }

        public IEnumerable<ValidationResult> IsValid(EventFooter baseEvent)
        {
            if (String.IsNullOrEmpty(baseEvent.Description))
                yield return new ValidationResult("Invalid Description.");

            if (String.IsNullOrEmpty(baseEvent.Logo))
                yield return new ValidationResult("Invalid Logo.");

            if (String.IsNullOrEmpty(baseEvent.Title))
                yield return new ValidationResult("Invalid Title.");

            if (!HelperMethod.IsValidUrl(baseEvent.Website))
                yield return new ValidationResult("Invalid Website.");
            
        }
 
    }
}