using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InTheLoopAPI.Models.Database;


namespace InTheLoopAPI.Models.Database
{
    public class Follow
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public string FollowingId { get; set; }
    }
}