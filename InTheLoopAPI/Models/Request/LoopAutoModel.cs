using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Models.Request
{
    public class LoopAutoModel
    {
        public String LoopName { get; set; }

        public bool Following { get; set; }

        public string ImageUrl { get; set; }
    }
}