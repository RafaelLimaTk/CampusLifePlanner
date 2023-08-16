using AutoMapper;
using CampusLifePlanner.Application.DTOs;
using CampusLifePlanner.Application.Interfaces;
using CampusLifePlanner.Application.Services.Base;
using CampusLifePlanner.Domain.Entities;
using CampusLifePlanner.Domain.Interfaces;
using CampusLifePlanner.Infra.Data.Context;
using CampusLifePlanner.Infra.Data.Repositories.Base;

namespace CampusLifePlanner.Application.Services;

public class CourseService : GenericService<CourseDto, Course>, ICourseService
{
    private readonly ICourseRepository repository;
    private readonly IMapper _mapper;

    public CourseService(ICourseRepository repository, IMapper mapper) : base(repository, mapper)
    {
        this.repository = repository;
        _mapper = mapper;
    }

    public bool ExistEvent(Guid id)
    {
        return repository.ExistEvent(id);
    }

    public IList<CourseDto> GetCourseListByCourseId(IList<Guid> enrollmentCourseIdList)
    {
        return _mapper.Map<List<CourseDto>>(repository.GetCourseListByCourseId(enrollmentCourseIdList));
    }
}
