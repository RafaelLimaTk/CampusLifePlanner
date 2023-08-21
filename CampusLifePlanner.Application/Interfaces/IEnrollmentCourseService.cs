using CampusLifePlanner.Application.DTOs;
using CampusLifePlanner.Application.Interfaces.Base;
using CampusLifePlanner.Domain.Entities;

namespace CampusLifePlanner.Application.Interfaces;

public interface IEnrollmentCourseService : IService<EnrollmentCourseDto, EnrollmentCourse>
{
    IList<EnrollmentCourse> GetListByUserId(Guid userId);
    bool HasEnrollmentCourseByUserId(Guid userId);
}
