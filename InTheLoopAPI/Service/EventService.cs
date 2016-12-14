using InTheLoopAPI.Models;
using InTheLoopAPI.Models.Database;
using InTheLoopAPI.Models.Request;
using InTheLoopAPI.Models.RequestModels;
using InTheLoopAPI.Queries;
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

        public ValidationResult DeletePublishedEvent(EventModel eventModel, string userId)
        {
            if (eventModel != null && eventModel.Id != 0)
            {
                var eventHeader = _repository.EventHeaders.SingleOrDefault(x => x.Id == eventModel.Id);

                if (eventHeader == null)
                {
                    return new ValidationResult("Event not found");
                }

                if (eventHeader.EventFooter.UserId != userId)
                {
                    return new ValidationResult("This is not your event GET AWAY!");
                }

                eventHeader.Archived = true;

                _repository.SaveChanges();
                
                return ValidationResult.Success;
            }

            return new ValidationResult("Event not removed");
        }

        public ValidationResult DeletePartialEvent(EventModel eventModel, string userId)
        {
            if(eventModel != null && eventModel.Id != 0)
            {
                var eventHeader = _repository.EventHeaders.SingleOrDefault(x => x.Id == eventModel.Id);

                if (eventHeader == null)
                {
                    return new ValidationResult("Event not found");
                }

                if (eventHeader.EventFooter.UserId != userId)
                {
                    return new ValidationResult("This is not your event GET AWAY!");
                }

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

            if (eventModel.Loops != null)
            {
                foreach (LoopModel t in eventModel.Loops)
                {
                    t.LoopName = t.LoopName.Trim();

                    var tag = _repository.Loops.SingleOrDefault(x => x.Name.ToLower() == t.LoopName.ToLower());

                    if (tag == null)
                    {
                        tag = new Loop { Name = t.LoopName };

                        eventHeader.EventLoops.Add(new EventLoop { Loop = tag });
                    }
                    else
                        eventHeader.EventLoops.Add(new EventLoop { Loop = tag });
                }
            }

            _repository.EventHeaders.Add(eventHeader);
            _repository.SaveChanges();

            var model = eventHeader.ToEventModel(userId);

            return model;
        }

        public EventModel AddOrUpdatePartialEvent(string userId, EventModel eventModel)
        {

            if (eventModel.Id <= 0 || eventModel.EventFooterId <= 0)
            {
                return AddPartialEvent(userId, eventModel);
            }

            var eventHeader = _repository.EventHeaders
                .Include("TagEvents")
                .Include("Flags")
                .Include("Attendees")
                .SingleOrDefault(x => x.Id == eventModel.Id);

            if (eventHeader == null)
                return null;

            _repository.EventLoops.RemoveRange(eventHeader.EventLoops);

            foreach (LoopModel t in eventModel.Loops)
            {
                var tag = _repository.Loops.SingleOrDefault(x => x.Name == t.LoopName);

                if (tag == null)
                {
                    tag = new Loop { Name = t.LoopName };

                    eventHeader.EventLoops.Add(new EventLoop { Loop = tag });
                }
                else
                    eventHeader.EventLoops.Add(new EventLoop { Loop = tag });
            }

            // UPDATE EVENT HEADER AND FOOTER

            eventHeader.Replace(eventModel.ToEventHeader(userId));

            _repository.SaveChanges();

            var model = eventHeader.ToEventModel(userId);

            return model;
        }

        public List<EventModel> GetEvents(string userId, double latitude, double longitude, double radius, FilterModel model)
        {
            return _eventRepository.GetEvents(userId, latitude, longitude, radius, model);
        }

        public List<EventModel> GetUserEvents(string userId)
        {
            return _eventRepository.GetUserEvents(userId, false);
        }

        public List<EventModel> GetHomeEvents(string userId, double latitude, double longitude, double radius)
        {
            var results = _eventRepository.GetHomeEvents(userId, latitude, longitude, radius);

            return results;
        }

        public List<EventModel> GetMostPopularToday(string userId, double latitude, double longitude, double radius, DateTime today)
        {
            var results = _eventRepository.GetMostPopularToday(userId, latitude, longitude, radius, today);

            return results;
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
    }
}