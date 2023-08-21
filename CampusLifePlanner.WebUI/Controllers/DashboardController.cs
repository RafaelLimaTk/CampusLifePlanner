using AutoMapper;
using CampusLifePlanner.Application.DTOs;
using CampusLifePlanner.Application.Interfaces;
using CampusLifePlanner.Domain.Entities;
using CampusLifePlanner.WebUI.ViewModels;
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
            try
            {
                var events = await _eventService.GetAllAsync();
                var today = FilterEventByDate(events, DateTime.Today.Date);
                var nextDay = FilterEventByDate(events, DateTime.Today.Date.AddDays(1));

                var todayEventDto = _mapper.Map<IEnumerable<EventDto>>(today);
                var nextDayEventDto = _mapper.Map<IEnumerable<EventDto>>(nextDay);

                var model = new EventViewModel
                {
                    TodayEvents = todayEventDto,
                    NextDayEvents = nextDayEventDto
                };

                return View(model);
            }
            catch (Exception ex)
            {
                return Json(new { success = true, message = $"Ocorreu um erro ao buscar os eventos {ex}" , type = "error" });
            }
        }

        private IEnumerable<Event> FilterEventByDate(IEnumerable<Event> events, DateTime date)
        {
            return events.Where(e => e.StartDate.Date <= date && e.EndDate.Date >= date);
        }
    }
}
