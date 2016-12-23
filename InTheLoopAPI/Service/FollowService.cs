using InTheLoopAPI.Models;
using InTheLoopAPI.Models.Database;
using InTheLoopAPI.Models.Request;
using InTheLoopAPI.Models.RequestModels;
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

        public Boolean IsFollowingUser(String myUserId, String theirUserId)
        {
            return _databaseContext.Follows.Any(x => x.UserId == myUserId && x.FollowingId == theirUserId);
        }

        public LoopModel GetLoop(string loopName, string userId, double latitude, double longitude, double radius)
        {

            double degrees = radius / 69;
            double maxLat = latitude + degrees;
            double minLat = latitude - degrees;
            double maxLong = longitude + degrees;
            double minLong = longitude - degrees;

            var loop = _databaseContext.Loops
                .Include("EventLoops")
                .Include("EventLoops.EventHeader.Attendees")
                .Include("EventLoops.EventHeader.EventLoops.Loop")
                .Include("UserLoops")
                .Include("UserLoops.User.MyLoops")
                .Include("UserLoops.User.Followers")
                .SingleOrDefault(x => x.Name.ToLower() == loopName.ToLower());

            var followers = _databaseContext.UserLoops
                .Where(x => x.LoopId == loop.Id)
                .Select(x => x.User)
                .ToList();

            if (loop == null)
            {
                return null;
            }

            var model = new LoopModel();

            var currentEvents = loop.EventLoops
                .Where(x => 
                    x.EventHeader.End.CompareTo(DateTime.UtcNow) >= 0 &&
                    x.EventHeader.Archived == false &&
                    x.EventHeader.Published == true 
                )
                .Select(x => x.EventHeader)
                .ToList();

            var localEvents = currentEvents.Where(x =>
                x.Latitude > minLat &&
                x.Latitude < maxLat &&
                x.Longitude > minLong &&
                x.Longitude < maxLong
            ).ToList();

            model.Followers = loop.UserLoops.Select(y => new UserModelLite
            {
                UserId = y.User.Id,
                IsFollowing = y.User.Followers.ToList().Any(f => f.UserId == userId),
                Username = y.User.UserName,
                ProfileImageURL = y.User.ImageURL
            }).ToList();
            model.ImageUrl = loop.ImageUrl;
            model.LoopId = loop.Id;
            model.LoopName = loop.Name;
            model.Following = loop.UserLoops.Any(t => t.UserId == userId);
            model.AllEvents = ToEventModelList(currentEvents, userId);
            model.LocalEvents = ToEventModelList(localEvents, userId);

            return model;
        }

        public List<LoopModel> GetLoops(String userId)
        {
            return _databaseContext.Loops.Where(x => x.UserLoops.Any(t => t.UserId == userId))
                .Select(tm => new LoopModel
                {
                    LoopName = tm.Name,
                    LoopId = tm.Id,
                    Following = true,
                    ImageUrl = tm.ImageUrl
                })
                .ToList();
        }

        public ValidationResult UnfollowLoop(String userId, String name)
        {
            name = name.Trim();

            var userLoop = _databaseContext.UserLoops
                .SingleOrDefault(t => t.Loop.Name.ToLower() == name.ToLower() && t.UserId == userId);

            if (userLoop == null)
                return new ValidationResult("User was not following this Loop");

            _databaseContext.UserLoops.Remove(userLoop);

            _databaseContext.SaveChanges();

            return ValidationResult.Success;
        }

        public List<LoopAutoModel> LoopAutocomplete(String loopName, String userId)
        {
            return _databaseContext.Loops.Where(x =>
                x.Name.StartsWith(loopName) ||
                x.Name.EndsWith(loopName) ||
                x.Name.Contains(loopName))
                .Select(s => new LoopAutoModel
                {
                    LoopName = s.Name,
                    Following = s.UserLoops.Any(t => t.UserId == userId),
                    ImageUrl = s.ImageUrl

                }).ToList();
        }

        public List<UserModelLite> UserAutocomplete(String term, String userId)
        {
            return _followRepository.UsersAutoComplete(userId, term);
        }

        public ValidationResult FollowLoop(String userId, LoopModel Loop)
        {
            Loop.LoopName = Loop.LoopName.Trim();

            var existingLoop = _databaseContext.Loops.SingleOrDefault(x => x.Name.ToLower() == Loop.LoopName.ToLower());

            if (existingLoop == null)
                return new ValidationResult("No Loop found with this name");

            var existingLoopForUser = _databaseContext.UserLoops.Any(x => x.LoopId == existingLoop.Id && x.UserId == userId);

            if (existingLoopForUser)
                return new ValidationResult("Uses is already following this Loop");

            var userLoop = new UserLoop { UserId = userId, LoopId = existingLoop.Id };

            _databaseContext.UserLoops.Add(userLoop);

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

        public List<UserModelLite> GetFollowers(string userId, string currentUser)
        {
            return _followRepository.GetFollowers(userId, currentUser);
        }

        public List<UserModelLite> GetFollowing(string userId, string currentUser)
        {
            return _followRepository.GetFollowing(userId, currentUser);
        }

        public List<EventModel> ToEventModelList(List<EventHeader> eventHeaders, string userId)
        {
            var modelList = new List<EventModel>();

            foreach (var _event in eventHeaders)
            {
                modelList.Add(_event.ToEventModel(userId));
            }

            return modelList;
        }
    }
}