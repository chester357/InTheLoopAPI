using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Models.Request
{
    public class UserModelLite
    {
        public String UserId { get; set; }

        public String Username { get; set; }

        public string ProfileImageURL { get; set; }

        public bool IsFollowing { get; set; }
    }
}