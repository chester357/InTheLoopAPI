using InTheLoopAPI.Models;
using InTheLoopAPI.Queries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Service.Validation
{
    public class EventValidator : BaseQuery
    {
        public IEnumerable<ValidationResult> IsValid(Event newEvent)
        {
            return new List<ValidationResult>();
        }

        public IEnumerable<ValidationResult> IsValid(BaseEvent baseEvent)
        {
            return new List<ValidationResult>();
        }
    }
}