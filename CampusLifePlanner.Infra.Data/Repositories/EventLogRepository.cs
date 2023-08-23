using CampusLifePlanner.Domain.Entities;
using CampusLifePlanner.Domain.Interfaces;
using CampusLifePlanner.Infra.Data.Context;
using CampusLifePlanner.Infra.Data.Repositories.Base;

namespace CampusLifePlanner.Infra.Data.Repositories;

public class EventLogRepository : Repository<EventLog>, IEventLogRepository
{
    private readonly ApplicationDbContext _context;
    public EventLogRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public EventLog FindByEventAndUserId(Guid eventId, Guid userId)
    {
        return _context.EventLogs
            .Where(el => el.EventId == eventId && el.UserId == userId)
            .FirstOrDefault();
    }

    public bool GetEventCompletedStatus(Guid eventId, Guid userId)
    {
        var eventLog = _context.EventLogs
            .Where(el => el.EventId == eventId && el.UserId == userId)
            .FirstOrDefault();

        return eventLog?.Completed ?? false;
    }
}
