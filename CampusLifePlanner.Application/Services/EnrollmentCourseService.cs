using AutoMapper;
using CampusLifePlanner.Application.DTOs;
using CampusLifePlanner.Application.Interfaces;
using CampusLifePlanner.Application.Services.Base;
using CampusLifePlanner.Domain.Entities;
using CampusLifePlanner.Domain.Interfaces;

namespace CampusLifePlanner.Application.Services;

public class EnrollmentCourseService : GenericService<EnrollmentCourseDto, EnrollmentCourse>, IEnrollmentCourseService
{
    private readonly IEnrollmentCourseRepository _repository;
    private readonly IMapper _mapper;

    public EnrollmentCourseService(IEnrollmentCourseRepository repository, IMapper mapper) : base(repository, mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public IList<EnrollmentCourse> GetListByUserId(Guid userId)
        => _repository.GetListByUserId(userId);

    public bool HasEnrollmentCourseByUserId(Guid userId)
        => _repository.HasEnrollmentCourseByUserId(userId);
}
