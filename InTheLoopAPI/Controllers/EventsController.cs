using InTheLoopAPI.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.OData;

namespace InTheLoopAPI.Controllers
{
    public class EventsController : ApiController
    {
        public EventService _service;

        public EventsController()
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
        public IHttpActionResult GetEvent(double lat, double lon, double radius)
        {
            try
            {
                var results = _service.GetEvents(lat, lon ,radius);

                if (!results.Any())
                    return BadRequest();

                return Ok(results);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
