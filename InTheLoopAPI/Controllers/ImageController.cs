using InTheLoopAPI.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using InTheLoopAPI.App_Start;
using System.Web.UI.WebControls;
using System.IO;

namespace InTheLoopAPI.Controllers
{
    //[Authorize, RequireHttps]
    [Authorize]
    public class ImageController : ApiController
    {
        ImageService _imageService;

        public ImageController()
        {
            _imageService = new ImageService();
        }

        [HttpPost, Route("api/Images")]
        public IHttpActionResult PostImage()
        {
            try
            {
                var path = _imageService.UploadImage(HttpContext.Current.Request.Files, User.Identity.GetUserId());

                _imageService.UpdateProfileImage(User.Identity.GetUserId(), path);

                return Ok(new ImageResponse { ImageURL = path });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet, Route("api/StockPhotos/{category}")]
        public IHttpActionResult GetStockPhotos(string category)
        {
            try
            {
                return Ok(_imageService.GetStockPhotos(category));
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        public class ImageResponse
        {
            public String ImageURL { get; set; }
        }
    }
}
