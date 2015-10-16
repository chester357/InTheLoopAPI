using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Models.Request
{
    public class ResetPasswordModel
    {
        public String Token { get; set; }

        public String EmailAddress { get; set; }

        public String NewPassword { get; set; }
    }
}