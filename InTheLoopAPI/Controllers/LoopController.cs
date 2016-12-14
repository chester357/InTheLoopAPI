using InTheLoopAPI.App_Start;
using InTheLoopAPI.Models.Request;
using InTheLoopAPI.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace InTheLoopAPI.Controllers
{
#if !DEBUG
    [RequireHttps]
#endif
    public class LoopController : ApiController
    {
        LoopService _service;

        public LoopController()
        {
            _service = new LoopService();
        }

        [HttpPost, Route("api/Loop")]
        public IHttpActionResult PostLoop(LoopModel tagModel)
        {
            try
            {
                var result = _service.CreateLoop(tagModel);

                if(result == null)
                {
                    return BadRequest();
                }

                return Ok(result);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
