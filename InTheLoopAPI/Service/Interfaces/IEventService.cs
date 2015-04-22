using InTheLoopAPI.Models.RequestModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InTheLoopAPI.Service.Interfaces
{
    public interface IEventService
    {
        List<ValidationResult> AddNewEvent(string userId, EventModel model);

        List<ValidationResult> AddRepeatEvent(string userId, RepeatEventModel model);

        List<EventModel> GetEvents(double latitude, double longitude, int radius);

        EventModel GetEvent(int eventId);

        ValidationResult ArchiveEvent(int eventId);

        List<ValidationResult> UpdateEvent(EventModel eventModel, int eventId);
    }
}
