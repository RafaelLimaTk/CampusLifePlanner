using CampusLifePlanner.Application.DTOs;
using CampusLifePlanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusLifePlanner.Application.Factories.Interfaces;

public interface IEventDtoFactory
{
    EventDto CreateEventDto(Event e, Guid userId, bool completed);
}
