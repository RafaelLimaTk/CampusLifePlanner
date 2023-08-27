using CampusLifePlanner.Application.DTOs;
using CampusLifePlanner.Application.Interfaces.Base;
using CampusLifePlanner.Domain.Entities;

namespace CampusLifePlanner.Application.Interfaces;

public interface ICourseService : IService<CourseDto, Course>
{
    bool ExistEvent(Guid id);
    IList<CourseDto> GetCourseListByCourseId(IList<Guid> enrollmentCourseIdList);
    Task<int> GetEnrollmentCountByCourseId(Guid courseId);
}
