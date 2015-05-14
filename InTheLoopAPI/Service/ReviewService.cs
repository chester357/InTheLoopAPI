using InTheLoopAPI.Models;
using InTheLoopAPI.Models.Request;
using InTheLoopAPI.Queries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Service
{
    public class ReviewService
    {
        private DatabaseContext _databaseContext;
        private EventRepository _eventRepository;
        private AttendedEventRepository _attendedEventRepository;

        public ReviewService()
        {
            _databaseContext = new DatabaseContext();
            _eventRepository = new EventRepository();
            _attendedEventRepository = new AttendedEventRepository();
        }

        public ValidationResult SetReview(ReviewModel review, string userId)
        {
            if (!_eventRepository.ValidEventId(review.EventId))
                return new ValidationResult("Invalid Event Id");

            if (review.Rating < 1|| review.Rating > 5)
                return new ValidationResult("Invalid Rating");

            AttendedEvent attendedEvent = _databaseContext.AttendedEvents
                .SingleOrDefault(x => x.EventId == review.EventId && x.UserId == userId);

            if (attendedEvent == null)
            {
                attendedEvent = new AttendedEvent
                {
                    EventId = review.EventId,
                    Image = review.Image,
                    Liked = review.Liked,
                    Rating = review.Rating,
                    Review = review.Review,
                    UserId = userId
                };

                _databaseContext.AttendedEvents.Add(attendedEvent);
            }
            else
            {
                    attendedEvent.Image = review.Image;
                    attendedEvent.Liked = review.Liked; 
                    attendedEvent.Rating = review.Rating;
                    attendedEvent.Review = review.Review; 
            }

            _databaseContext.SaveChanges();

            return ValidationResult.Success;
        }

        public List<ReviewModel> GetReviews(int baseEventId)
        {
            return _attendedEventRepository.GetReviews(baseEventId);
        }

        public ReviewModel GetReview(int attendedEventId)
        {
            return _attendedEventRepository.GetReview(attendedEventId);
        }
    
    }
}