using InTheLoopAPI.Models.Database;
using InTheLoopAPI.Models.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Models.Request
{
    public class LoopModel
    {
        public String LoopName { get; set; }

        public int LoopId { get; set; }

        public string ImageUrl { get; set; }

        public bool Following { get; set; }

        public List<UserModelLite> Followers { get; set; }
    
        public List<EventModel> LocalEvents { get; set; }

        public List<EventModel> AllEvents { get; set; }

        public static List<Loop> GetStarterLoops()
        {
            var loops = new List<Loop>();

            loops.Add(new Loop
            {
                Name = "Live Music"
            });

            loops.Add(new Loop
            {
                Name = "Live Comedy"
            });

            loops.Add(new Loop
            {
                Name = "Wine & Spirits"
            });

            loops.Add(new Loop
            {
                Name = "Festival"
            });

            loops.Add(new Loop
            {
                Name = "Shopping"
            });

            loops.Add(new Loop
            {
                Name = "Eating Out"
            });

            return loops;
        }
    }
}