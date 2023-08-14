using AutoMapper;
using CampusLifePlanner.Application.DTOs;
using CampusLifePlanner.Domain.Entities;

namespace CampusLifePlanner.Application.Mappings;

public class DomainToDTOMappingProfile : Profile
{
    public DomainToDTOMappingProfile()
    {
        CreateMap<Event, EventDto>().ReverseMap();
        CreateMap<Course, CourseDto>().ReverseMap();
        CreateMap<User, UserDto>().ReverseMap();
    }
}
