using AutoMapper;
using CampusLifePlanner.Application.DTOs;
using CampusLifePlanner.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CampusLifePlanner.WebUI.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IEventService _eventService;
        private readonly IMapper _mapper;

        public DashboardController(IEventService eventService, IMapper mapper)
        {
            _eventService = eventService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var events = await _eventService.GetAllAsync();
            var today = DateTime.Today.Date;
            var filteredEvents = events.Where(e => e.StartDate.Date <= today && e.EndDate.Date >= today);
            var eventDto = _mapper.Map<IEnumerable<EventDto>>(filteredEvents);

            return View(eventDto);
        }
    }
}
