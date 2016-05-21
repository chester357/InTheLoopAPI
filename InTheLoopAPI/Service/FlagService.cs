using InTheLoopAPI.Models;
using InTheLoopAPI.Models.Database;
using InTheLoopAPI.Models.Request;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Service
{
    public class FlagService
    {
        private DatabaseContext _repository;

        public FlagService()
        {
            _repository = new DatabaseContext();
        }

        public ValidationResult FlagEvent(FlagModel flagModel, string userId)
        {
            var hasFlagged = _repository.Flags.Any(f => f.UserId == userId && f.EventHeaderId == flagModel.EventHeaderId);

            if (hasFlagged)
            {
                return new ValidationResult("You cannot flag an event more than once.");
            }

            var flag = new FlagEvent
            {
                Date = DateTime.UtcNow,
                EventHeaderId = flagModel.EventHeaderId,
                Message = flagModel.Message == null ? "" : flagModel.Message,
                Severity = flagModel.Severity,
                UserId = userId
            };

            _repository.Flags.Add(flag);

            _repository.SaveChanges();

            return ValidationResult.Success;
        }
    }
}