using InTheLoopAPI.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace InTheLoopAPI.Controllers
{
    public class EmailController : ApiController
    {
        EmailService service;

        public EmailController()
        {
            service = new EmailService();
        }

    }
}
