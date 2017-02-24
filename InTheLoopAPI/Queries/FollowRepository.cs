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
            return Follows.SingleOrDefault(x => x.FollowingMeId == userId && x.ImFollowingId == followingId);
        }

        public List<UserModelLite> GetFollowers(string userId, string currentUser)
        {
            return Follows
                .Where(x => x.ImFollowingId == userId)
                .Select(y => new UserModelLite
                {
                    UserId = y.FollowingMe.Id,
                    IsFollowing = Follows.Any(f => f.FollowingMeId == currentUser && f.ImFollowingId == y.FollowingMeId),
                    Username = y.FollowingMe.UserName,
                    ProfileImageURL = y.FollowingMe.ImageURL
                })
                .ToList();

            //return Follows
            //    .Where(x => x.FollowingId == userId)
            //    .Select(y => new UserModelLite
            //    {
            //        UserId = y.UserId,
            //        IsFollowing = y.User.Followers.Any(f => f.UserId == currentUser),
            //        Username = y.User.UserName,
            //        ProfileImageURL = y.User.ImageURL
            //    })
            //    .ToList();
        }

        public List<UserModelLite> GetFollowing(string userId, string currentUser)
        {

            //return Users.Where(x => x.Followers.Any(f => f.UserId == userId))
            //    .Select(y => new UserModelLite
            //    {
            //        UserId = .Id,
            //        IsFollowing = y.Followers.Any(f => f.UserId == currentUser),
            //        Username = y.UserName,
            //        ProfileImageURL = y.ImageURL
            //    })
            //    .ToList();


            // TODO: Find a way to make this faster
            return Follows
                .Where(x => x.FollowingMeId == userId)
                .Select(y => new UserModelLite
                {
                    UserId = y.ImFollowing.Id,
                    IsFollowing = Follows.Any(f => f.FollowingMeId == currentUser && f.ImFollowingId == y.ImFollowingId),
                    Username = y.ImFollowing.UserName,
                    ProfileImageURL = y.ImFollowing.ImageURL
                })
                .ToList();
        }

        public List<UserModelLite> UsersAutoComplete(string userId, string search)
        {
            return MyUsers
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
                    IsFollowing = Follows.Any(f => f.FollowingMeId == userId && f.ImFollowingId == u.Id),
                    Username = u.UserName
                })
                .ToList();
        }

        public bool IsFollowing(string userId, string followingId)
        {
            return Follows.Any(x => x.FollowingMeId == userId && x.ImFollowingId == followingId);
        }
    }
}