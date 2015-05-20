using InTheLoopAPI.Models;
using InTheLoopAPI.Models.Request;
using InTheLoopAPI.Queries;
using InTheLoopAPI.Service.Validation;
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
        private AttendanceRepository _attendanceRepository;
        private ReviewValidator _validator;

        public ReviewService()
        {
            _databaseContext = new DatabaseContext();
            _eventRepository = new EventRepository(_databaseContext);
            _attendanceRepository = new AttendanceRepository(_databaseContext);
            _validator = new ReviewValidator(_eventRepository);
        }

        public ValidationResult SetReview(ReviewModel review, string userId)
        {
            var result = _validator.SetReview(review, userId);

            if (result != ValidationResult.Success)
                return result;

            Attendance attendedEvent = _attendanceRepository.GetAttendance(review.EventHeaderId, userId);

            List<ReviewImage> images = new List<ReviewImage>();

            if (review.Images != null)
                review.Images.ForEach(x => images.Add(new ReviewImage { Image = x }));

            if (attendedEvent == null)
            { 
                attendedEvent = new Attendance
                {
                    EventHeaderId = review.EventHeaderId,
                    ReviewImages = images,
                    Liked = review.Liked,
                    Rating = review.Rating,
                    Review = review.Review,
                    UserId = userId
                };

                _databaseContext.Attendances.Add(attendedEvent);
            }
            else
            {
                    attendedEvent.ReviewImages = images;
                    attendedEvent.Liked = review.Liked; 
                    attendedEvent.Rating = review.Rating;
                    attendedEvent.Review = review.Review; 
            }

            _databaseContext.SaveChanges();

            return result;
        }

        public List<ReviewModel> GetReviews(int baseEventId)
        {
            return _attendanceRepository.GetReviews(baseEventId);
        }

        public ReviewModel GetReview(int attendedEventId)
        {
            return _attendanceRepository.GetReview(attendedEventId);
        }
    
    }
}