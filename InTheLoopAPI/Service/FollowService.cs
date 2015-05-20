using InTheLoopAPI.Models;
using InTheLoopAPI.Models.Database;
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
    public class FollowService
    {
        private DatabaseContext _databaseContext;
        private FollowRepository _followRepository;
        private UserRepository _userRepository;
        private FollowValidator _validator;

        public FollowService()
        {
            _databaseContext = new DatabaseContext();
            _followRepository = new FollowRepository(_databaseContext);
            _userRepository = new UserRepository(_databaseContext);
            _validator = new FollowValidator(_followRepository, _userRepository);    
        }

        public ValidationResult AddFollower(string userId, string followingId)
        {
            var result = _validator.AddFollower(userId, followingId);

            if (result != ValidationResult.Success)
                return result;

            var follow = new Follow { UserId = userId, FollowingId = followingId };

            _databaseContext.Follows.Add(follow);

            _databaseContext.SaveChanges();

            return result;
        }

        public ValidationResult StopFollowing(string userId, string followingId)
        {
            var result = _validator.StopFollowing(userId, followingId);

            if (result != ValidationResult.Success)
                return result;

            var follow = _followRepository.GetFollower(userId, followingId);

            _databaseContext.Follows.Remove(follow);

            _databaseContext.SaveChanges();

            return result;
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