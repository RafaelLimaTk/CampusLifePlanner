using AutoMapper;
using CampusLifePlanner.Application.DTOs;
using CampusLifePlanner.Application.Interfaces;
using CampusLifePlanner.Application.Services.Base;
using CampusLifePlanner.Domain.Entities;
using CampusLifePlanner.Domain.Interfaces;

namespace CampusLifePlanner.Application.Services;

public class CourseService : GenericService<CourseDto, Course>, ICourseService
{
    public CourseService(ICourseRepository repository, IMapper mapper) : base(repository, mapper)
    {
    }
}
