using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Models.Request
{
    public class UserModel
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public String ImageURL { get; set; }

        public int Loops { get; set; }

        public int FollowingCount { get; set; }

        public int FollowersCount { get; set; }

        public int TagCount { get; set; }

        public int EventCount { get; set; }

        public string Quote { get; set; }

    }
}