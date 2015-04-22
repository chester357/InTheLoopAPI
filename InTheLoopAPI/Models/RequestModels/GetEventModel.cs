using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Models.RequestModels
{
    public class GetEventModel
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public int Radius { get; set; }
    }
}