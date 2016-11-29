using InTheLoopAPI.Models;
using InTheLoopAPI.Models.Request;
using InTheLoopAPI.Queries;
using InTheLoopAPI.Service.Validation;
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
        private AttendanceValidator _validator;
        
        public AttendanceService()
        {
            _databaseContext = new DatabaseContext();
            _attendanceRepository = new AttendanceRepository(_databaseContext);
            _eventRepository = new EventRepository(_databaseContext);
            _validator = new AttendanceValidator(_eventRepository, _attendanceRepository);
        }

        public ValidationResult PlueOne(string userId, int eventHeaderId)
        {
            var result = _validator.PlusOne(userId, eventHeaderId);

            if (result != ValidationResult.Success)
                return result;

            var attendance = new Attendance { EventHeaderId = eventHeaderId, UserId = userId };

            _databaseContext.Attendances.Add(attendance);

            var evnt = _databaseContext.EventHeaders.SingleOrDefault(x => x.Id == eventHeaderId);

            evnt.Loops++;

            _databaseContext.SaveChanges();

            return ValidationResult.Success;
        }

        public ValidationResult RemoveAttendance(string userId, int eventHeaderId)
        {
            var result = _validator.RemoveAttendance(userId, eventHeaderId);

            if (result != ValidationResult.Success)
                return result;

            var attendance = _attendanceRepository.GetAttendance(eventHeaderId, userId);

            _databaseContext.Attendances.Remove(attendance);

            var evnt = _databaseContext.EventHeaders.SingleOrDefault(x => x.Id == eventHeaderId);

            evnt.Loops--;

            _databaseContext.SaveChanges();

            return ValidationResult.Success;
        }

        public int GetAttendanceCount(int eventHeaderId)
        {
            return _attendanceRepository.GetCount(eventHeaderId);
        }

        public List<UserModelLite> GetAttendies(int eventHeaderId, string currentUser)
        {
            return _attendanceRepository.GetAttendies(eventHeaderId, currentUser);
        }
    }
}