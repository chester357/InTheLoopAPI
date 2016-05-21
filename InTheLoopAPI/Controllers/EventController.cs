using InTheLoopAPI.Models.RequestModels;
using InTheLoopAPI.Service;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.OData;
using Microsoft.AspNet.Identity;
using InTheLoopAPI.Helpers;
using System.ComponentModel.DataAnnotations;
using InTheLoopAPI.Models.Request;

namespace InTheLoopAPI.Controllers
{
    [Authorize]
    //[Authorize, RequireHttps]
    public class EventController : ApiController
    {
        public EventService _service;

        public EventController()
        {
            _service = new EventService();
        }

        [AllowAnonymous]
        [HttpGet, Route("api/Event/{eventId}")]
        public IHttpActionResult GetEvent(int eventId)
        {
            try
            {
                var result = _service.GetEvent(User.Identity.GetUserId() ,eventId);

                if (result == null)
                    return BadRequest();

                return Ok(result);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [AllowAnonymous]
        [HttpGet, EnableQuery, Route("api/Event/Latitude/{lat}/Longitude/{lon}/Radius/{radius}")]
        public IHttpActionResult GetEvents(double lat, double lon, double radius)
        {
            try
            {
                return Ok(_service.GetEvents(User.Identity.GetUserId() ,lat, lon ,radius));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet, EnableQuery, Route("api/Event/Home/Latitude/{lat}/Longitude/{lon}/Radius/{radius}")]
        public IHttpActionResult GetHomeEvents(double lat, double lon, double radius)
        {
            try
            {
                return Ok(_service.GetHomeEvents(User.Identity.GetUserId(), lat, lon, radius));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet, EnableQuery, Route("api/Event/MostPopularToday/Latitude/{lat}/Longitude/{lon}/Radius/{radius}/Today/{today}")]
        public IHttpActionResult MostPopularToday(double lat, double lon, double radius, string today)
        {
            try
            {
                var date = DateTime.Parse(today);

                return Ok(_service.GetMostPopularToday(User.Identity.GetUserId(), lat, lon, radius, date));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet, EnableQuery, Route("api/Event/MostPopularThisWeekend/Latitude/{lat}/Longitude/{lon}/Radius/{radius}/Today/{today}")]
        public IHttpActionResult GetMostPopularThisWeekend(double lat, double lon, double radius, DateTime today)
        {
            try
            {
                return Ok(_service.GetMostPopularToday(User.Identity.GetUserId(), lat, lon, radius, today));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet, EnableQuery, Route("api/Event/User/{userId}")]
        public IHttpActionResult GetUserHomeEvents(string userId)
        {
            try
            {
                var evnts = _service.GetUserEvents(User.Identity.GetUserId());

                return Ok(evnts);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost, Route("api/Event")]
        public IHttpActionResult PostEvent(EventModel eventModel)
        {
            try
            {
                var results = _service.AddEvent(User.Identity.GetUserId(), eventModel);

                if (results.Any())
                    return BadRequest(HelperMethod.DisplayErrors(results));

                return Ok();
            }
            catch(Exception ex)
            {
               return  InternalServerError(ex);
            }
        }

        [HttpPost, Route("api/Event/Partial")]
        public IHttpActionResult PostPartialEvent(EventModel eventModel)
        {
            try
            {
                var results = _service.AddOrUpdatePartialEvent(User.Identity.GetUserId(), eventModel);

                if (results == null)
                    return BadRequest();

                return Ok(results);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete, Route("api/Event/Partial")]
        public IHttpActionResult DeletePartialEvent(EventModel eventModel)
        {
            try
            {
                var result = _service.DeletePartialEvent(eventModel);

                if(result == ValidationResult.Success)
                {
                    return Ok();
                }

                return BadRequest();
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut, Route("api/Event/Partial")]
        public IHttpActionResult UpdatePartialEvent(EventModel eventModel)
        {
            try
            {
                var results = _service.AddOrUpdatePartialEvent(User.Identity.GetUserId(), eventModel);

                if (results == null)
                    return BadRequest();

                return Ok(results);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet, Route("api/Event/Publish")]
        public IHttpActionResult GetMyPublishedEvents()
        {
            try
            {
                var results = _service.GetMyPublishedEvents(User.Identity.GetUserId());

                return Ok(results);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete, Route("api/Event/Publish")]
        public IHttpActionResult DeletePublishedEvent(EventModel eventModel)
        {
            try
            {
                var result = _service.DeletePartialEvent(eventModel);

                if (result == ValidationResult.Success)
                {
                    return Ok();
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet, Route("api/Event/Partial")]
        public IHttpActionResult GetPartialEvents()
        {
            try
            {
                var results = _service.GetPartialEvents(User.Identity.GetUserId());

                return Ok(results);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut, Route("api/Event/Partial/Publish")]
        public IHttpActionResult PublishPartialEvent(EventModel eventModel)
        {
            try
            {
                var result = _service.PublishEvent(eventModel, User.Identity.GetUserId());

                if(result == ValidationResult.Success)
                {
                    return Ok();
                }

                return BadRequest(result.ErrorMessage);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost, Route("api/Event/Header")]
        public IHttpActionResult PostEvent(EventHeaderModel repeatEventModel)
        {
            try
            {
                var results = _service.AddEventHeader(User.Identity.GetUserId(), repeatEventModel);

                if (results.Any())
                    return BadRequest(HelperMethod.DisplayErrors(results));

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost, Route("api/Event/UpdateViewCount/{eventId}")]
        public IHttpActionResult UpdateEventCount(int eventId)
        {
            try
            {
                _service.UpdateViewCount(eventId);

                return Ok();
            }
            catch
            {
                return InternalServerError();
            }
        }

        [HttpPut, Route("api/Event/Archive/{eventHeaderId}")]
        public IHttpActionResult ArchiveEvent(int eventHeaderId)
        {
            try
            {
                var result = _service.ArchiveEvent(eventHeaderId, User.Identity.GetUserId());

                if (result != ValidationResult.Success)
                    return BadRequest(result.ErrorMessage);

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut, Route("api/Event/Header")]
        public IHttpActionResult UpdateEventHeader(EventHeaderModel headerModel)
        {
            try
            {
                var results = _service.UpdateEventHeader(headerModel, User.Identity.GetUserId());

                if (results.Any())
                    return BadRequest(HelperMethod.DisplayErrors(results));

                return Ok();
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut, Route("api/Event/Footer")]
        public IHttpActionResult UpdateEventFooter(EventFooterModel footerModel)
        {
            try
            {
                var results = _service.UpdateEventFooter(footerModel, User.Identity.GetUserId());

                if (results.Any())
                    return BadRequest(HelperMethod.DisplayErrors(results));

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
