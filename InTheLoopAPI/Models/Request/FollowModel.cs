using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Models.Request
{
    public class FollowModel
    {
        public string UserId { get; set; }

        public string Username { get; set; }

        public string ProfileImageURL { get; set; }
    }
}