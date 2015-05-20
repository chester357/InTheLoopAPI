using InTheLoopAPI.Queries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Service.Validation
{
    public class AttendanceValidator
    {
        private EventRepository _eventRepository;
        private AttendanceRepository _attendanceRepository;

        public AttendanceValidator(EventRepository er, AttendanceRepository ar)
        {
            _eventRepository = er;
            _attendanceRepository = ar;
        }

        public ValidationResult PlusOne(string userId, int eventHeaderId)
        {
            if (!_eventRepository.ValidEventHeaderId(eventHeaderId))
                return new ValidationResult("Invalid Event Id");

            else if (_attendanceRepository.IsAttending(eventHeaderId, userId))
                return new ValidationResult("You are already attending this event");

            else
                return ValidationResult.Success;
        }

        public ValidationResult RemoveAttendance(string userId, int eventHeaderId)
        {
            if (!_attendanceRepository.IsAttending(eventHeaderId, userId))
                return new ValidationResult("You are not currently attending this event");
            else
                return ValidationResult.Success;
        }
    }
}