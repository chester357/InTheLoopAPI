using InTheLoopAPI.Models.Request;
using InTheLoopAPI.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using InTheLoopAPI.App_Start;

namespace InTheLoopAPI.Controllers
{
    [Authorize, RequireHttps]
    public class ReviewController : ApiController
    {
        private ReviewService _reviewService;

        public ReviewController()
        {
            _reviewService = new ReviewService();
        }

        [HttpGet, Route("api/Review/{attendedEventId}")] 
        public IHttpActionResult GetReview(int attendedEventId)
        {
            try
            {
                var result = _reviewService.GetReview(attendedEventId);

                if (result == null)
                    return BadRequest("No review found");

                return Ok(result);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet, Route("api/Review/{baseEventId}")]
        public IHttpActionResult GetReviews(int baseEventId)
        {
            try
            {
                var result = _reviewService.GetReviews(baseEventId);

                if (!result.Any())
                    return BadRequest("No reviews found");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    
        [HttpPost, Route("api/Review")]
        public IHttpActionResult PostReview(ReviewModel reviewModel)
        {
            try
            {
                var result = _reviewService.SetReview(reviewModel, User.Identity.GetUserId());

                if (result != null)
                    return BadRequest(result.ErrorMessage);

                return Ok();
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut, Route("api/Review")]
        public IHttpActionResult PutReview(ReviewModel reviewModel)
        {
            try
            {
                var result = _reviewService.SetReview(reviewModel, User.Identity.GetUserId());

                if (result != null)
                    return BadRequest(result.ErrorMessage);

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
