using InTheLoopAPI.Helpers;
using InTheLoopAPI.Models;
using InTheLoopAPI.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Queries
{
    public class AttendanceRepository : BaseQuery
    {
        public List<ReviewModel> GetReviews(int baseEventId)
        {
            return Attendances
                .Where(x => x.Event.BaseEventId == baseEventId && x.Rating > 0)
                .Select(y => new ReviewModel
                {
                    EventId = y.EventHeaderId,
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
            return Attendances
                .Where(x => x.Id == attendedEventId)
                .Select(y => new ReviewModel
                {
                    EventId = y.EventHeaderId,
                    Id = y.Id,
                    Image = y.Image,
                    Liked = y.Liked,
                    Rating = y.Rating,
                    Review = y.Review
                })
                .SingleOrDefault();
        }

        public bool IsAttending(int eventHeaderId, string userId)
        {
            return Database.Attendances.Any(x => x.EventHeaderId == eventHeaderId && x.UserId == userId);
        }

        public Attendance GetAttendance(int eventHeaderId, string userId)
        {
            return Attendances.SingleOrDefault(x => x.EventHeaderId == eventHeaderId && x.UserId == userId);
        }

        public int GetCount(int eventHeaderId)
        {
            return Attendances.Count(x => x.EventHeaderId == eventHeaderId);
        }

        public List<UserModel> GetAttendies(int eventHeaderId)
        {
            return Attendances
                .Where(x => x.EventHeaderId == eventHeaderId)
                .Select(y => new UserModel
                {
                    Email = y.User.Email,
                    Image = HelperMethod.ByteArrayToString(y.User.Image),
                    Quote = y.User.Quote,
                    UserId = y.UserId,
                    UserName = y.User.UserName
                })
                .ToList();
        }
    }
}