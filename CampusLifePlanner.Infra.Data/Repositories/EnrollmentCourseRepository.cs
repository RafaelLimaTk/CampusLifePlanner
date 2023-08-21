using CampusLifePlanner.Domain.Entities;
using CampusLifePlanner.Domain.Interfaces;
using CampusLifePlanner.Infra.Data.Context;
using CampusLifePlanner.Infra.Data.Repositories.Base;

namespace CampusLifePlanner.Infra.Data.Repositories;

public class EnrollmentCourseRepository : Repository<EnrollmentCourse>, IEnrollmentCourseRepository
{
    private readonly ApplicationDbContext _context;
    public EnrollmentCourseRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public IList<EnrollmentCourse> GetListByUserId(Guid userId)
        => _context.EnrollmentCourse.Where(a => a.UserId == userId).ToList(); 

    public bool HasEnrollmentCourseByUserId(Guid userId)
     => _context.EnrollmentCourse.Any(a => a.UserId == userId);
}
