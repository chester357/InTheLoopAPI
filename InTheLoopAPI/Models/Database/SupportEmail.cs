using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InTheLoopAPI.Models.Database;

namespace InTheLoopAPI.Models.Database
{
    public class SupportEmail
    {
        public int Id { get; set; }

        public string Message { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}