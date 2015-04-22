using InTheLoopAPI.Models.RequestModels;
using InTheLoopAPI.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace InTheLoopAPI.Controllers
{
    public class EventController : ApiController
    {
        private IEventService _service;

        public EventController(IEventService service)
        {
            _service = service;
        }

        [HttpGet, Route("api/Events")]
        public IHttpActionResult GetEvents(GetEventModel getEventModel)
        {
            return null;
        }

        [HttpGet, Route("api/Event/{eventId}")]
        public IHttpActionResult GetEvent(int eventId)
        {
            return null;
        }

        [HttpPost, Route("api/Event")]
        public IHttpActionResult PostEvent(EventModel eventModel)
        {
            return null;
        }

        [HttpPost, Route("api/Event/Repeat")]
        public IHttpActionResult PostRepeatEvent(RepeatEventModel repeatEventModel)
        {
            return null;
        }

        [HttpPut, Route("api/Event/Archive/{eventId}")]
        public IHttpActionResult ArchiveEvent(int eventId)
        {
            return null;
        }

        [HttpPut, Route("api/Event/{eventId}")]
        public IHttpActionResult UpdateEvent(EventModel eventModel, int eventId)
        {
            return null;
        }
    }
}
