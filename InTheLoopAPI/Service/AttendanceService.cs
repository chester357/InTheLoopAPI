using InTheLoopAPI.Models;
using InTheLoopAPI.Models.Request;
using InTheLoopAPI.Queries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Service
{
    public class AttendanceService
    {
        private DatabaseContext _databaseContext;
        private AttendanceRepository _attendanceRepository;
        private EventRepository _eventRepository;
        
        public AttendanceService()
        {
            _databaseContext = new DatabaseContext();
            _attendanceRepository = new AttendanceRepository();
            _eventRepository = new EventRepository();
        }

        public ValidationResult PlueOne(string userId, int eventHeaderId)
        {
            if (!_eventRepository.ValidEventHeaderId(eventHeaderId))
                return new ValidationResult("Invalid Event Id");

            if (_attendanceRepository.IsAttending(eventHeaderId, userId))
                return new ValidationResult("You are already attending this event");

            var attendance = new Attendance { EventHeaderId = eventHeaderId, UserId = userId };

            _databaseContext.Attendances.Add(attendance);

            _databaseContext.SaveChanges();

            return ValidationResult.Success;
        }

        public ValidationResult RemoveAttendance(string userId, int eventHeaderId)
        {
            var attendance = _databaseContext.Attendances
                .SingleOrDefault(x => x.EventHeaderId == eventHeaderId && x.UserId == userId);
            
            if (attendance == null)
                return new ValidationResult("You are not currently attending this event");

            _databaseContext.Attendances.Remove(attendance);

            _databaseContext.SaveChanges();

            return ValidationResult.Success;
        }

        public int GetAttendanceCount(int eventHeaderId)
        {
            return _attendanceRepository.GetCount(eventHeaderId);
        }

        public List<UserModel> GetAttendies(int eventHeaderId)
        {
            return _attendanceRepository.GetAttendies(eventHeaderId);
        }
    }
}