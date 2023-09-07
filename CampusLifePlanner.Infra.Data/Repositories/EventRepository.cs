using CampusLifePlanner.Domain.Entities;
using CampusLifePlanner.Domain.Interfaces;
using CampusLifePlanner.Infra.Data.Context;
using CampusLifePlanner.Infra.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace CampusLifePlanner.Infra.Data.Repositories;

public class EventRepository : Repository<Event>, IEventRepository
{
    public EventRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Event>> GetEventsByCourse(Guid courseId)
        => await Entities.Where(e => e.CourseId == courseId).ToListAsync();

    public async Task<IList<Event>> GetEventsWithCoursesAsync()
        => await Entities.Include(a => a.Course).ToListAsync();

    public async Task<Event> GetWithCourseById(Guid id)
     => await Entities.Where(x => x.Id == id).Include(a => a.Course).FirstOrDefaultAsync();

    public async Task<Event> GetByIdAsNoTrankingAsync(Guid id)
        => await Entities.Where(a => a.Id == id).AsNoTracking().FirstOrDefaultAsync();
}
