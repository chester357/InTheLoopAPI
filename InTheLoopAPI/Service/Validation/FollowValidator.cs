using InTheLoopAPI.Queries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Service.Validation
{
    public class FollowValidator
    {
        private FollowRepository _followRepository;
        private UserRepository _userRepository;

        public FollowValidator(FollowRepository fr, UserRepository ur)
        {
            _followRepository = fr;
            _userRepository = ur;
        }

        public ValidationResult AddFollower(string userId, string followingId)
        {
            if (!_userRepository.ValidUserId(followingId))
                return new ValidationResult("Invalid user to follow");

            else if (_followRepository.IsFollowing(userId, followingId))
                return new ValidationResult("Already following this user");

            else
                return ValidationResult.Success;
        }

        public ValidationResult StopFollowing(string userId, string followingId)
        {
            if (!_followRepository.IsFollowing(userId, followingId))
                return new ValidationResult("Invalid user to stop following");
            else
                return ValidationResult.Success;
        }
    }
}