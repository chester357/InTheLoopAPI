using InTheLoopAPI.Models;
using InTheLoopAPI.Models.Database;
using InTheLoopAPI.Models.Request;
using InTheLoopAPI.Queries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Service
{
    public class FollowService
    {
        DatabaseContext _databaseContext;
        FollowerRepository _followRepository;

        public FollowService()
        {
            _databaseContext = new DatabaseContext();
            _followRepository = new FollowerRepository();
        }

        public ValidationResult AddFollower(string userId, string followingId)
        {
            if (!_databaseContext.Users.Any(x => x.Id == followingId))
                return new ValidationResult("Invalid user to follow");

            if (!_databaseContext.Follows.Any(x => x.UserId == userId && x.FollowingId == followingId))
                return new ValidationResult("Already following this user");


            var follow = new Follow { UserId = userId, FollowingId = followingId };

            _databaseContext.Follows.Add(follow);

            _databaseContext.SaveChanges();

            return ValidationResult.Success;
        }

        public ValidationResult StopFollowing(string userId, string followingId)
        {
            var follow = _followRepository.GetFollower(userId, followingId);

            if (follow == null)
                return new ValidationResult("Invalid user to stop following");

            _databaseContext.Follows.Remove(follow);

            _databaseContext.SaveChanges();

            return ValidationResult.Success;
        }

        public List<UserModel> GetFollowers(string userId)
        {
            return _followRepository.GetFollowers(userId);
        }

        public List<UserModel> GetFollowing(string userId)
        {
            return _followRepository.GetFollowing(userId);
        }
    }
}