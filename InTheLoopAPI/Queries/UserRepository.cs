using InTheLoopAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Queries
{
    public class UserRepository : BaseQuery
    {
        public UserRepository(DatabaseContext db) : base(db)
        {

        }

        public bool ValidUserId(string userId)
        {
            return Users.Any(x => x.Id == userId);
        }
    }
}