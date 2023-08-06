using AutoMapper;
using CampusLifePlanner.Application.DTOs;
using CampusLifePlanner.Application.Interfaces;
using CampusLifePlanner.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusLifePlanner.WebUI.Controllers
{
    [Authorize]
    public class EventController : Controller
    {
        private readonly IEventService _eventService;
        private readonly IMapper _mapper;

        public EventController(IEventService eventService, IMapper mapper)
        {
            _eventService = eventService;   
            _mapper = mapper;
        }

        public async Task<IActionResult> GetEvents()
        {
            var events = await _eventService.GetAllAsync();
            var eventDtos = _mapper.Map<IEnumerable<EventDto>>(events);
            return Json(eventDtos);
        }

        [HttpPost]

        public async Task<IActionResult> CreateEvent(EventDto eventDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _eventService.CreateAsync(eventDto);

            return Ok();
        }
    }
}
