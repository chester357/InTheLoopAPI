using InTheLoopAPI.Models.Request;
using InTheLoopAPI.Queries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Service.Validation
{
    public class ReviewValidator
    {
        private EventRepository _eventRepository;

        public ReviewValidator(EventRepository er)
        {
            _eventRepository = er;
        }

        public ValidationResult SetReview(ReviewModel review, string userId)
        {
            if (!_eventRepository.ValidEventHeaderId(review.EventHeaderId))
                return new ValidationResult("Invalid Event Id");

            else if (review.Rating < 1 || review.Rating > 5)
                return new ValidationResult("Invalid Rating");

            else return ValidationResult.Success;
        }

    }
}