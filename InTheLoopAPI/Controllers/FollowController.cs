using InTheLoopAPI.Models.Request;
using InTheLoopAPI.Service;
using System;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;
using InTheLoopAPI.App_Start;
using System.Web.Http.OData;

namespace InTheLoopAPI.Controllers
{
    [Authorize]
    //[Authorize, RequireHttps]
    public class FollowController : ApiController
    {
        FollowService _followService;

        public FollowController()
        {
            _followService = new FollowService();
        }

        [HttpDelete, EnableQuery, Route("api/Follow/Tag/{name}")]
        public IHttpActionResult UnfollowEventTag(String name)
        {
            try
            {
                var result = _followService.UnfollowTag(User.Identity.GetUserId(), name);

                if (result != null)
                    return BadRequest(result.ErrorMessage);

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost, EnableQuery, Route("api/Follow/Tag")]
        public IHttpActionResult FollowEventTag(TagModel tag)
        {
            try
            {
                var result = _followService.FollowTag(User.Identity.GetUserId(), tag);

                if (result != null)
                    return BadRequest(result.ErrorMessage);

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet, EnableQuery, Route("api/Follow/Tag")]
        public IHttpActionResult GetFollowTags()
        {
            try
            {
                return Ok(_followService.GetTags(User.Identity.GetUserId()));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet, Route("api/Follow/User/Followers")]
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

        [HttpGet, Route("api/Follow/User/Following")]
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

        [HttpPost, Route("api/Follow/User")]
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

        [HttpDelete, Route("api/Follow/User")]
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
