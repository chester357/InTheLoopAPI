using InTheLoopAPI.Models;
using InTheLoopAPI.Models.Database;
using InTheLoopAPI.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Queries
{
    public class FollowRepository : BaseQuery
    {
        public Follow GetFollower(string userId, string followingId)
        {
            return Follows.SingleOrDefault(x => x.UserId == userId && x.FollowingId == followingId);
        }

        public List<UserModel> GetFollowers(string userId)
        {
            return Follows
                .Where(x => x.FollowingId == userId)
                .Select(y => new UserModel
                {
                    Email = y.User.Email,
                    ProfilePic = y.User.Image,
                    Quote = y.User.Quote,
                    UserId = y.UserId,
                    UserName = y.User.UserName
                })
                .ToList();
        }

        public List<UserModel> GetFollowing(string userId)
        {
            return Follows
                .Where(x => x.UserId == userId)
                .Select(y => new UserModel
                {
                    Email = y.User.Email,
                    ProfilePic = y.User.Image,
                    Quote = y.User.Quote,
                    UserId = y.UserId,
                    UserName = y.User.UserName
                })
                .ToList();
        }
    }
}