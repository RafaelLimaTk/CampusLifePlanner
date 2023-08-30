using CampusLifePlanner.Domain.Entities;
using CampusLifePlanner.Domain.Interfaces.Base;

namespace CampusLifePlanner.Domain.Interfaces;

public interface IEventRepository : IRepository<Event>
{
    Task<IEnumerable<Event>> GetEventsByCourse(Guid courseId);
    Task<IList<Event>> GetEventsWithCoursesAsync();
    Task<Event> GetWithCourseById(Guid id);
    Task<Event> GetByIdAsNoTrankingAsync(Guid id);
}
