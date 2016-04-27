﻿using InTheLoopAPI.Models.Request;
using InTheLoopAPI.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace InTheLoopAPI.Controllers
{
    public class TagController : ApiController
    {
        TagService _service;

        public TagController()
        {
            _service = new TagService();
        }

        [HttpPost, Route("api/Tag")]
        public IHttpActionResult PostTag(TagModel tagModel)
        {
            try
            {
                var result = _service.CreateTag(tagModel);

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