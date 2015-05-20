using InTheLoopAPI.Models.Request;
using InTheLoopAPI.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;

namespace InTheLoopAPI.Controllers
{
    [Authorize]
    public class FollowController : ApiController
    {
        FollowService _followService;

        public FollowController()
        {
            _followService = new FollowService();
        }

        [HttpGet, Route("api/Followers")]
        public IHttpActionResult GetFollowers()
        {
            try
            {
                var results = _followService.GetFollowers(User.Identity.GetUserId());

                if (results.Any())
                    return Ok(results);

                return BadRequest("No followers found");
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet, Route("api/Following")]
        public IHttpActionResult GetFollowing()
        {
            try
            {
                var results = _followService.GetFollowing(User.Identity.GetUserId());

                if (results.Any())
                    return Ok(results);

                return BadRequest("This user is not following anyone");
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost, Route("api/Follow")]
        public IHttpActionResult PostFollower(FollowModel followingModel)
        {
             try
            {
                var result = _followService.AddFollower(User.Identity.GetUserId(), followingModel.UserId);

                if (result == ValidationResult.Success)
                    return Ok();

                return BadRequest(result.ErrorMessage);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete, Route("api/Follow")]
        public IHttpActionResult StopFollowing(FollowModel followingModel)
        {
             try
            {
                var result = _followService.StopFollowing(User.Identity.GetUserId(), followingModel.UserId);

                if (result == ValidationResult.Success)
                    return Ok(result);

                return BadRequest(result.ErrorMessage);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
