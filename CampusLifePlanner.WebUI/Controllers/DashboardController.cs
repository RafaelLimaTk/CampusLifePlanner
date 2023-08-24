using AutoMapper;
using CampusLifePlanner.Application.DTOs;
using CampusLifePlanner.Application.Interfaces;
using CampusLifePlanner.Domain.Entities;
using CampusLifePlanner.Infra.Data.Identity;
using CampusLifePlanner.WebUI.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CampusLifePlanner.WebUI.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IEventService _eventService;
        private readonly IEventLogService _eventLogService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public DashboardController(IEventService eventService, IEventLogService eventLogService, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _eventService = eventService;
            _eventLogService = eventLogService;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var userId = _userManager.FindByNameAsync(User.Identity.Name).Result.Id;
                var events = await _eventService.GetAllAsync();

                var today = _eventLogService.FilterMapEvents(events, DateTime.Today.Date, Guid.Parse(userId));
                var nextDay = _eventLogService.FilterMapEvents(events, DateTime.Today.Date.AddDays(1), Guid.Parse(userId));

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

        [HttpPost]
        public void CreateEventLog(Guid eventId, bool isChecked)
        {
            var userId = _userManager.FindByNameAsync(User.Identity.Name).Result.Id;

            _eventLogService.ToggleEventLog(eventId, Guid.Parse(userId), isChecked);
        }

        private IEnumerable<Event> FilterEventByDate(IEnumerable<Event> events, DateTime date)
        {
            return events.Where(e => e.StartDate.Date <= date && e.EndDate.Date >= date);
        }
    }
}
