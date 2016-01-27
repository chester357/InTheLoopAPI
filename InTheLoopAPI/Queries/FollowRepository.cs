using InTheLoopAPI.Helpers;
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
        public FollowRepository(DatabaseContext db) : base(db)
        {

        }

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
                    //Email = y.User.Email,
                    ImageURL = y.User.ImageURL,
                    //Quote = y.User.Quote,
                    UserId = y.UserId,
                    UserName = y.User.UserName
                })
                .ToList();
        }

        public List<UserModel> GetFollowing(string userId)
        {
            // TODO: Find a way to make this faster
            return Follows
                .Where(x => x.UserId == userId)
                .Select(y => new UserModel
                {
                    //Email = Users.SingleOrDefault(u => u.Id == y.FollowingId).Email
                    ImageURL = Users.FirstOrDefault(u => u.Id == y.FollowingId).ImageURL,
                    //Quote = y.User.Quote,
                    UserId = Users.FirstOrDefault(u => u.Id == y.FollowingId).Id,
                    UserName = Users.FirstOrDefault(u => u.Id == y.FollowingId).UserName
                })
                .ToList();
        }

        public List<UserModelLite> UsersAutoComplete(string userId, string search)
        {
            return Users
                .Where
                (x =>
                    x.Id != userId &&
                    (
                        x.UserName.Contains(search) ||
                        x.UserName.EndsWith(search) ||
                        x.UserName.StartsWith(search)
                    )
                )
                .Select(u => new UserModelLite
                {
                    UserId = u.Id,
                    ProfileImageURL = u.ImageURL,
                    IsFollowing = Follows.Any(f => f.UserId == userId && f.FollowingId == u.Id),
                    Username = u.UserName
                })
                .ToList();
        }

        public bool IsFollowing(string userId, string followingId)
        {
            return Follows.Any(x => x.UserId == userId && x.FollowingId == followingId);
        }
    }
}