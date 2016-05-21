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
    public class FlagController : ApiController
    {
        FlagService service;

        public FlagController()
        {
            service = new FlagService();
        }

        [HttpPost, Route("api/Flag")]
        public IHttpActionResult FlagEvent(FlagModel model)
        {
            try
            {
                var result = service.FlagEvent(model, User.Identity.GetUserId());

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
    }
}
