using CampusLifePlanner.Domain.Entities;
using CampusLifePlanner.Domain.Interfaces;
using CampusLifePlanner.Infra.Data.Context;
using CampusLifePlanner.Infra.Data.Repositories.Base;

namespace CampusLifePlanner.Infra.Data.Repositories;

public class CourseRepository : Repository<Course>, ICourseRepository
{
    public CourseRepository(ApplicationDbContext context) : base(context)
    {
    }

    public bool ExistEvent(Guid id)
    {
        return Entities.Any(e => e.Id == id);
    }

    public IList<Course> GetCourseListByCourseId(IList<Guid> enrollmentCourseIdList)
    {
        return Entities.Where(a => enrollmentCourseIdList.Contains(a.Id)).ToList();
    }
}
