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
    {
        return await Entities.Where(e => e.CourseId == courseId).ToListAsync();
    }
}
