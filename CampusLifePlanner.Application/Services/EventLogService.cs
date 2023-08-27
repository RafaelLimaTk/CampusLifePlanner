using AutoMapper;
using CampusLifePlanner.Application.DTOs;
using CampusLifePlanner.Application.Interfaces;
using CampusLifePlanner.Application.Services.Base;
using CampusLifePlanner.Domain.Entities;
using CampusLifePlanner.Domain.Interfaces;

namespace CampusLifePlanner.Application.Services;

public class EventLogService : GenericService<EventLogDto, EventLog>, IEventLogService
{
    private readonly IEventLogRepository _eventLogRepository;
    private readonly IMapper _mapper;
    public EventLogService(IEventLogRepository repository, IMapper mapper) : base(repository, mapper)
    {
        _eventLogRepository = repository;
        _mapper = mapper;
    }

    public IEnumerable<EventDto> FilterMapEvents(IEnumerable<Event> events, DateTime date, Guid userId, IList<Guid> courseIds)
    {
        return FilterEventByDate(FilterEventByCourse(events, courseIds), date)
        .Select(e => new EventDto
        {
            Id = e.Id,
            Title = e.Title,
            Description = e.Description,
            Local = e.Local,
            StartDate = e.StartDate,
            EndDate = e.EndDate,
            CourseId = e.CourseId,
            Completed = GetEventCompletedStatus(e.Id, userId)
        });
    }

    private IEnumerable<Event> FilterEventByDate(IEnumerable<Event> events, DateTime date)
    {
        return events.Where(e => e.StartDate.Date <= date && e.EndDate.Date >= date);
    }

    private IEnumerable<Event> FilterEventByCourse(IEnumerable<Event> events, IList<Guid> courseIds)
    {
        return events.Where(e => courseIds.Contains(e.CourseId));
    }

    public async Task ToggleEventLog(Guid eventId, Guid userId, bool isMarked)
    {
        var existingEventLog = _eventLogRepository.FindByEventAndUserId(eventId, userId);

        if (existingEventLog == null && isMarked)
        {
            await CreateEventLogAsync(eventId, userId);
        }
        else
        {
            await UpdateEventLogAsync(existingEventLog, isMarked);
        }
    }

    private async Task CreateEventLogAsync(Guid eventId, Guid userId)
    {
        var eventLog = new EventLog
        {
            UserId = userId,
            EventId = eventId,
            Completed = true,
        };

        await _eventLogRepository.CreateAsync(eventLog);
    }

    private async Task UpdateEventLogAsync(EventLog existingEventLog, bool isMarked)
    {
        existingEventLog.Completed = isMarked;
        await _eventLogRepository.UpdateAsync(existingEventLog);
    }

    public bool GetEventCompletedStatus(Guid eventId, Guid userId)
    {
        return _eventLogRepository.GetEventCompletedStatus(eventId, userId);
    }
}
