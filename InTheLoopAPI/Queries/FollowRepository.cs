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

        public List<UserModelLite> GetFollowers(string userId, string currentUser)
        {
            return Follows
                .Where(x => x.FollowingId == userId)
                .Select(y => new UserModelLite
                {
                    UserId = y.User.Id,
                    IsFollowing = Follows.Any(f => f.UserId == currentUser && f.FollowingId == y.UserId),
                    Username = y.User.UserName,
                    ProfileImageURL = y.User.ImageURL
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
                .Where(x => x.UserId == userId)
                .Select(y => new UserModelLite
                {
                    UserId = y.Following.Id,
                    IsFollowing = Follows.Any(f => f.UserId == currentUser && f.FollowingId == y.FollowingId),
                    Username = y.Following.UserName,
                    ProfileImageURL = y.Following.ImageURL
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