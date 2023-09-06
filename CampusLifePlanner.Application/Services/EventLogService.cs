using AutoMapper;
using CampusLifePlanner.Application.DTOs;
using CampusLifePlanner.Application.Factories.Interfaces;
using CampusLifePlanner.Application.Interfaces;
using CampusLifePlanner.Application.Services.Base;
using CampusLifePlanner.Domain.Entities;
using CampusLifePlanner.Domain.Interfaces;

namespace CampusLifePlanner.Application.Services;

public class EventLogService : GenericService<EventLogDto, EventLog>, IEventLogService
{
    private readonly IEventLogRepository _eventLogRepository;
    private readonly IEventDtoFactory _eventDtoFactory;
    private readonly IMapper _mapper;
    public EventLogService(IEventLogRepository repository, IEventDtoFactory eventDtoFactory, IMapper mapper) : base(repository, mapper)
    {
        _eventLogRepository = repository;
        _eventDtoFactory = eventDtoFactory;
        _mapper = mapper;
    }

    public IEnumerable<EventDto> FilterMapEvents(IEnumerable<Event> events, DateTime date, Guid userId, IList<Guid> courseIds)
    {
        var filteredEvents = FilterEvents(events, date, courseIds);
        return MapToEventDtos(filteredEvents, userId);
    }

    #region Methods Extensions FilterMapEvents
    private IEnumerable<Event> FilterEvents(IEnumerable<Event> events, DateTime date, IList<Guid> courseIds)
    {
        return FilterEventByDate(FilterEventByCourse(events, courseIds), date);
    }

    private IEnumerable<Event> FilterEventByDate(IEnumerable<Event> events, DateTime date)
    {
        return events.Where(e => e.StartDate.Date <= date && e.EndDate.Date >= date);
    }

    private IEnumerable<Event> FilterEventByCourse(IEnumerable<Event> events, IList<Guid> courseIds)
    {
        return events.Where(e => courseIds.Contains(e.CourseId));
    }

    private IEnumerable<EventDto> MapToEventDtos(IEnumerable<Event> events, Guid userId)
    {
        return events.Select(e => _eventDtoFactory.CreateEventDto(e, userId, GetEventCompletedStatus(e.Id, userId)));
    }
    #endregion

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
