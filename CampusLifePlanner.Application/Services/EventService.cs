using AutoMapper;
using CampusLifePlanner.Application.DTOs;
using CampusLifePlanner.Application.Interfaces;
using CampusLifePlanner.Application.Services.Base;
using CampusLifePlanner.Domain.Entities;
using CampusLifePlanner.Domain.Interfaces;

namespace CampusLifePlanner.Application.Services;

public class EventService : GenericService<EventDto, Event>, IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;
    public EventService(IEventRepository repository, IMapper mapper) : base(repository, mapper)
    {
        _eventRepository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EventDto>> GetAllByCourseIdAsync(Guid courseId)
    {
        var events = await _eventRepository.GetEventsByCourse(courseId);
        return events.Select(e => _mapper.Map<EventDto>(e));
    }
}
