using InTheLoopAPI.Models.RequestModels;
using InTheLoopAPI.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.OData;
using Microsoft.AspNet.Identity;
using InTheLoopAPI.Helpers;
using System.ComponentModel.DataAnnotations;
using InTheLoopAPI.Models.Request;
using InTheLoopAPI.Models;
using System.Threading.Tasks;
using System.Web;
using InTheLoopAPI.App_Start;

namespace InTheLoopAPI.Controllers
{
    [Authorize, RequireHttps]
    public class EventController : ApiController
    {
        public EventService _service;

        public EventController()
        {
            _service = new EventService();
        }

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

        [HttpGet, EnableQuery, Route("api/Events/Latitude/{lat}/Longitude/{lon}/Radius/{radius}")]
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
