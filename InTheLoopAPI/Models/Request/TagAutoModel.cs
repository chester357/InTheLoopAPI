using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Models.Request
{
    public class TagAutoModel
    {
        public String TagName { get; set; }

        public bool Following { get; set; }
    }
}