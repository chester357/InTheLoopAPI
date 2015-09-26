using InTheLoopAPI.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace InTheLoopAPI.Controllers
{
    public class ImageController : ApiController
    {
        ImageService _imageService;

        public ImageController()
        {
            _imageService = new ImageService();
        }

        [Authorize, HttpPost, Route("api/Images")]
        public IHttpActionResult PostImage()
        {
            try
            {
                var path = _imageService.UploadImage(HttpContext.Current.Request.Files, User.Identity.GetUserId());

                _imageService.UpdateProfileImage(User.Identity.GetUserId(), path);
                
                return Ok(path);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
