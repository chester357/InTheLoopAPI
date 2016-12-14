using InTheLoopAPI.Models;
using InTheLoopAPI.Models.Database;
using InTheLoopAPI.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Service
{
    public class LoopService
    {
        DatabaseContext _dataContext;

        public LoopService()
        {
            _dataContext = new DatabaseContext();
        }

        public Loop CreateLoop(LoopModel loopModel)
        {
            if(loopModel == null || string.IsNullOrEmpty(loopModel.LoopName))
            {
                return null;
            }

            var loop = new Loop { Name = loopModel.LoopName };

            var result = _dataContext.Loops.Add(loop);

            _dataContext.SaveChanges();

            return result;
        }
    }
}