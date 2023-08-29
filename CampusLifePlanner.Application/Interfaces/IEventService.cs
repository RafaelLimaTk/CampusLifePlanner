using CampusLifePlanner.Application.DTOs;
using CampusLifePlanner.Application.Interfaces.Base;
using CampusLifePlanner.Domain.Entities;

namespace CampusLifePlanner.Application.Interfaces;

public interface IEventService : IService<EventDto, Event>
{
    Task<IList<Event>> GetEventsWithCoursesAsync();
    Task<Event> GetWithCourseById(Guid id);
    Task<IEnumerable<EventDto>> GetAllByCourseIdAsync(Guid courseId);
    Task<bool> ShareEvent(Guid eventId, Guid targetCourseId);
}
