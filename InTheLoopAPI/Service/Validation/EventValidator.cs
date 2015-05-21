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

        public EventValidator(EventRepository er)
        {
            _eventRepository = er;
        }

        public IEnumerable<ValidationResult> EventHeader(EventHeader eventHeader, string userId)
        {
            if (!_eventRepository.ValidUserForEventFooter(userId, eventHeader.Id))
                yield return new ValidationResult("Invalid valid user for this event");

            if (String.IsNullOrEmpty(eventHeader.City))
                yield return new ValidationResult("Invalid City.");

            if (eventHeader.End.CompareTo(eventHeader.Start) <= 0)
                yield return new ValidationResult("Invalid Dates.");

            if (eventHeader.Latitude < -90 || eventHeader.Latitude > 90)
                yield return new ValidationResult("Invalid Latitude.");

            if (eventHeader.Longitude < -180 || eventHeader.Longitude > 180)
                yield return new ValidationResult("Invalid Longitude.");

            if (eventHeader.ZipCode.ToString().Length != 5)
                yield return new ValidationResult("Invalid Zip Code.");

            if (eventHeader.EventFooterId != 0)
                if (!_eventRepository.ValidEventFooterId(eventHeader.EventFooterId))
                    yield return new ValidationResult("Invalid Event Footer Id.");
        }

        public IEnumerable<ValidationResult> EventFooter(EventFooter baseEvent)
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
 
        public ValidationResult ArchiveEvent(int eventHeaderId, string userId)
        {
            if (!_eventRepository.ValidUserForEventHeader(userId, eventHeaderId))
                return new ValidationResult("You are not currently attending this event");
            else 
                return ValidationResult.Success;
        }
    }
}