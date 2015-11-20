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
                var result = _service.GetEvent(eventId);

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
                return Ok(_service.GetEvents(lat, lon ,radius));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost, EnableQuery, Route("api/Event/Home/Latitude/{lat}/Longitude/{lon}/Radius/{radius}")]
        public IHttpActionResult GetHomeEvents()
        {
            try
            {
                return Ok(_service.GetHomeEvents(User.Identity.GetUserId()));
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
