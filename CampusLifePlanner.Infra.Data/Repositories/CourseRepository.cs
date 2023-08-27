using CampusLifePlanner.Domain.Entities;
using CampusLifePlanner.Domain.Interfaces;
using CampusLifePlanner.Infra.Data.Context;
using CampusLifePlanner.Infra.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace CampusLifePlanner.Infra.Data.Repositories;

public class CourseRepository : Repository<Course>, ICourseRepository
{
    private readonly ApplicationDbContext _context;
    public CourseRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public bool ExistEvent(Guid id) => Entities.Any(e => e.Id == id);

    public IList<Course> GetCourseListByCourseId(IList<Guid> enrollmentCourseIdList) => 
        Entities.Where(a => enrollmentCourseIdList.Contains(a.Id)).ToList();

    public Task<int> GetEnrollmentCountByCourseId(Guid courseId) => 
        _context.EnrollmentCourse.CountAsync(c => c.CourseId == courseId);
}
