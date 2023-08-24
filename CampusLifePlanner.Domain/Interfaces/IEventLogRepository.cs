using CampusLifePlanner.Domain.Entities;
using CampusLifePlanner.Domain.Interfaces.Base;

namespace CampusLifePlanner.Domain.Interfaces;

public interface IEventLogRepository : IRepository<EventLog>
{
    EventLog FindByEventAndUserId(Guid eventId, Guid userId);
    bool GetEventCompletedStatus(Guid eventId, Guid userId);
}
