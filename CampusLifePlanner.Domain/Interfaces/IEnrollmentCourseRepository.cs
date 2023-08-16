using CampusLifePlanner.Domain.Entities;
using CampusLifePlanner.Domain.Interfaces.Base;

namespace CampusLifePlanner.Domain.Interfaces;

public interface IEnrollmentCourseRepository : IRepository<EnrollmentCourse>
{
    IList<EnrollmentCourse> GetListByUserId(Guid userId);
    bool HasEnrollmentCourseByUserId(Guid userId);
}
