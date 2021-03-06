﻿using InTheLoopAPI.Helpers;
using InTheLoopAPI.Models;
using InTheLoopAPI.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Queries
{
    public class    AttendanceRepository : BaseQuery
    {
        public AttendanceRepository(DatabaseContext db) : base(db)
        {

        }

        public List<ReviewModel> GetReviews(int baseEventId)
        {
            return Attendances
                .Where(x => x.Event.EventFooterId == baseEventId && x.Rating > 0)
                .Select(y => new ReviewModel
                {
                    EventHeaderId = y.EventHeaderId,
                    Id = y.Id,
                    Images = y.ReviewImages.Select(v => v.Image).ToList(),
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
                    EventHeaderId = y.EventHeaderId,
                    Id = y.Id,
                    Images = y.ReviewImages.Select(t => t.Image).ToList(),
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
                    ImageURL = y.User.ImageURL,
                    Quote = y.User.Quote,
                    UserId = y.UserId,
                    UserName = y.User.UserName
                })
                .ToList();
        }
    }
}