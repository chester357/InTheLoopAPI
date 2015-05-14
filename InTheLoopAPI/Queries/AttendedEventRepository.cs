using InTheLoopAPI.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Queries
{
    public class AttendedEventRepository : BaseQuery
    {
        public List<ReviewModel> GetReviews(int baseEventId)
        {
            return AttendedEvents
                .Where(x => x.Event.BaseEventId == baseEventId)
                .Select(y => new ReviewModel
                {
                    EventId = y.EventId,
                    Id = y.Id,
                    Image = y.Image,
                    Liked = y.Liked,
                    Rating = y.Rating,
                    Review = y.Review
                })
                .ToList();
        }

        public ReviewModel GetReview(int attendedEventId)
        {
            return AttendedEvents
                .Where(x => x.Id == attendedEventId)
                .Select(y => new ReviewModel
                {
                    EventId = y.EventId,
                    Id = y.Id,
                    Image = y.Image,
                    Liked = y.Liked,
                    Rating = y.Rating,
                    Review = y.Review
                })
                .SingleOrDefault();
        }
    }
}