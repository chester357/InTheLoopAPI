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

        public ValidationResult UnfollowTag(String userId, TagModel tag)
        {
            var userTag = _databaseContext.TagUsers.SingleOrDefault(t => t.Tag.Name == tag.TagName && t.UserId == userId);

            if (userTag == null)
                return new ValidationResult("User was not following this tag");

            _databaseContext.TagUsers.Remove(userTag);

            _databaseContext.SaveChanges();

            return ValidationResult.Success;
        }

        public ValidationResult FollowTag(String userId, TagModel tag)
        {
            var existingTag = _databaseContext.Tags.SingleOrDefault(x => x.Name == tag.TagName);

            if (existingTag == null)
                return new ValidationResult("No tag found with this name");

            var existingTagForUser = _databaseContext.TagUsers.Any(x => x.TagId == existingTag.Id && x.UserId == userId);

            if (existingTagForUser)
                return new ValidationResult("Uses is already following this tag");

            var userTag = new TagUser { UserId = userId, TagId = existingTag.Id };

            _databaseContext.TagUsers.Add(userTag);

            _databaseContext.SaveChanges();

            return ValidationResult.Success;
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