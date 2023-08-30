using AutoMapper;
using CampusLifePlanner.Application.DTOs;
using CampusLifePlanner.Domain.Entities;

namespace CampusLifePlanner.Application.Mappings;

public class DomainToDTOMappingProfile : Profile
{
    public DomainToDTOMappingProfile()
    {
        CreateMap<Event, EventDto>()
            .ForMember(dst => dst.Courses, opt => opt.MapFrom(x => x.Course == null ? null : x.Course))
            .ReverseMap();
        CreateMap<Course, CourseDto>().ReverseMap();
        CreateMap<EnrollmentCourse, EnrollmentCourseDto>().ReverseMap();
        CreateMap<EventLog, EventLogDto>().ReverseMap();
    }
}
