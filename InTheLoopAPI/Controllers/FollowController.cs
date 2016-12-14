using InTheLoopAPI.Models.Request;
using InTheLoopAPI.Service;
using System;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;
using InTheLoopAPI.App_Start;
using System.Web.Http.OData;
using System.Collections.Generic;

namespace InTheLoopAPI.Controllers
{
    [Authorize]
#if !DEBUG
    [RequireHttps]
#endif
    public class FollowController : ApiController
    {
        FollowService _followService;

        public FollowController()
        {
            _followService = new FollowService();
        }

        [HttpDelete, EnableQuery, Route("api/Follow/Loop/{name}")]
        public IHttpActionResult UnfollowEventLoop(String name)
        {
            try
            {
                var result = _followService.UnfollowLoop(User.Identity.GetUserId(), name);

                if (result != null)
                    return BadRequest(result.ErrorMessage);

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost, EnableQuery, Route("api/Follow/Loops/Unfollow")]
        public IHttpActionResult UnfollowEventTags(List<String> names)
        {
            try
            {
                ValidationResult result = null;

                foreach ( var name in names ){
                   result = _followService.UnfollowLoop(User.Identity.GetUserId(), name);
                }

                if (result != null)
                    return BadRequest(result.ErrorMessage);

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost, EnableQuery, Route("api/Follow/Loop")]
        public IHttpActionResult FollowEventLoop(LoopModel Loop)
        {
            try
            {
                var result = _followService.FollowLoop(User.Identity.GetUserId(), Loop);

                if (result != null)
                    return BadRequest(result.ErrorMessage);

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet, EnableQuery, Route("api/Follow/Loop/{loopName}/{lat}/{lon}/{radius}")]
        public IHttpActionResult GetLoop(string loopName, double lat, double lon, double radius)
        {
            try
            {
                var loop = _followService.GetLoop(loopName, User.Identity.GetUserId(), lat, lon, radius);

                if(loop == null)
                    return BadRequest();

                return Ok(loop);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet, Route("api/Follow/IsFollowing/{userId}")]
        public IHttpActionResult IsFollowing(String userId)
        {
            try
            {
                var result = _followService.IsFollowingUser(User.Identity.GetUserId(), userId);

                return Ok(new { IsFollowing = result });
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet, Route("api/Follow/Loop/Autocomplete")]
        public IHttpActionResult TagAutoCompleteAll()
        {
            try
            {
                var result = _followService.LoopAutocomplete("", User.Identity.GetUserId());

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet, Route("api/Follow/Loop/Autocomplete/{Loop}")]
        public IHttpActionResult TagAutoComplete(String Loop)
        {
            try
            {
                var results = _followService.LoopAutocomplete(Loop, User.Identity.GetUserId());

                return Ok(results);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet, Route("api/Follow/User/Autocomplete/{term}")]
        public IHttpActionResult UserAutoComplete(String term)
        {
            try
            {
                var results = _followService.UserAutocomplete(term, User.Identity.GetUserId());

                return Ok(results);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet, Route("api/Follow/User/Autocomplete")]
        public IHttpActionResult UserAutoCompleteAll()
        {
            try
            {
                var results = _followService.UserAutocomplete("", User.Identity.GetUserId());

                return Ok(results);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet, EnableQuery, Route("api/Follow/Loop/{userId}")]
        public IHttpActionResult GetFollowLoops(String userId)
        {
            try
            {
                return Ok(_followService.GetLoops(userId));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
              
        [HttpGet, Route("api/Follow/User/Followers/{userId}")]
        public IHttpActionResult GetFollowers(String userId)
        {
            try
            {
                var results = _followService.GetFollowers(userId, User.Identity.GetUserId());

                return Ok(results);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet, Route("api/Follow/User/Following/{userId}")]
        public IHttpActionResult GetFollowing(String userId)
        {
            try
            {
                var results = _followService.GetFollowing(userId, User.Identity.GetUserId());

                return Ok(results);
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

        [HttpDelete, Route("api/Follow/User/{userId}")]
        public IHttpActionResult StopFollowing(String userId)
        {
             try
            {
                var result = _followService.StopFollowing(User.Identity.GetUserId(), userId);

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
