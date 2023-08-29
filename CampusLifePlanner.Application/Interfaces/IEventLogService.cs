using CampusLifePlanner.Application.DTOs;
using CampusLifePlanner.Application.Interfaces.Base;
using CampusLifePlanner.Domain.Entities;

namespace CampusLifePlanner.Application.Interfaces;

public interface IEventLogService : IService<EventLogDto, EventLog>
{
    Task ToggleEventLog(Guid eventId, Guid userId, bool isMarked);
    bool GetEventCompletedStatus(Guid eventId, Guid userId);
    IEnumerable<EventDto> FilterMapEvents(IEnumerable<Event> events, DateTime date, Guid userId, IList<Guid> coursesIds);
}
