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

        public string FollowerId { get; set; }
        public virtual User Follower { get; set; }

        public string FollowieId { get; set; }
        public virtual User Followie{ get; set; }
    }
}