using CampusLifePlanner.Application.DTOs;
using CampusLifePlanner.Application.Factories.Interfaces;
using CampusLifePlanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusLifePlanner.Application.Factories.Implementations;

public class EventDtoFactory : IEventDtoFactory
{
    public EventDto CreateEventDto(Event e, Guid userId, bool completed)
    {
        return new EventDto
        {
            Id = e.Id,
            Title = e.Title,
            Description = e.Description,
            Local = e.Local,
            StartDate = e.StartDate,
            EndDate = e.EndDate,
            CourseId = e.CourseId,
            Courses = e.Course,
            JobId = e.JobId,
            Completed = completed
        };
    }
}
