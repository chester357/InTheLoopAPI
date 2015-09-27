using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Models
{
    public class Response
    {
        public bool Success { get; set; }

        public String Data { get; set; }

        public String Message { get; set; }
    }
}