using CampusLifePlanner.Domain.Entities;
using CampusLifePlanner.Domain.Interfaces.Base;

namespace CampusLifePlanner.Domain.Interfaces;

public interface ICourseRepository : IRepository<Course>
{
    bool ExistEvent(Guid id);
    IList<Course> GetCourseListByCourseId(IList<Guid> enrollmentCourseIdList);
    Task<int> GetEnrollmentCountByCourseId(Guid courseId);
}
