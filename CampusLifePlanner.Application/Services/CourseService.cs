using AutoMapper;
using CampusLifePlanner.Application.DTOs;
using CampusLifePlanner.Application.Interfaces;
using CampusLifePlanner.Application.Services.Base;
using CampusLifePlanner.Domain.Entities;
using CampusLifePlanner.Domain.Interfaces;

namespace CampusLifePlanner.Application.Services;

public class CourseService : GenericService<CourseDto, Course>, ICourseService
{
    private readonly IEnrollmentCourseService _enrollmentCourseService;
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;

    public CourseService(IEnrollmentCourseService enrollmentCourseService, ICourseRepository repository, IMapper mapper) : base(repository, mapper)
    {
        _enrollmentCourseService = enrollmentCourseService;
        _courseRepository = repository;
        _mapper = mapper;
    }

    public bool ExistEvent(Guid id)
    {
        return _courseRepository.ExistEvent(id);
    }

    public IList<CourseDto> GetCourseListByCourseId(IList<Guid> enrollmentCourseIdList)
    {
        return _mapper.Map<List<CourseDto>>(_courseRepository.GetCourseListByCourseId(enrollmentCourseIdList));
    }

    public async Task<int> GetEnrollmentCountByCourseId(Guid courseId)
    {
        return await _courseRepository.GetEnrollmentCountByCourseId(courseId);
    }
}
