using InTheLoopAPI.App_Start;
using InTheLoopAPI.Models.Database;
using InTheLoopAPI.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace InTheLoopAPI.Controllers
{
    public class EmailController : ApiController
    {
        EmailService service;

        public EmailController()
        {
            service = new EmailService();
        }

        [AllowAnonymous]
        [HttpPost, Route("api/email/support")]
        public IHttpActionResult PostSupportEmail(SupportEmail email)
        {
            try
            {
                return Ok(service.SendSupportEmail(email));
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
