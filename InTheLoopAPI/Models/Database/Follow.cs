using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InTheLoopAPI.Models.Database;
using System.ComponentModel.DataAnnotations.Schema;

namespace InTheLoopAPI.Models.Database
{
    public class Follow
    {
        public int Id { get; set; }

        public string FollowingMeId { get; set; }
        public virtual User FollowingMe { get; set; }

        public string ImFollowingId { get; set; }
        public virtual User ImFollowing { get; set; }
    }
}