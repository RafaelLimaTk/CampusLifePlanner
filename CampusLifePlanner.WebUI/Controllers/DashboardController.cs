using AutoMapper;
using CampusLifePlanner.Application.DTOs;
using CampusLifePlanner.Application.Interfaces;
using CampusLifePlanner.Infra.Data.Identity;
using CampusLifePlanner.WebUI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace CampusLifePlanner.WebUI.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly IEventService _eventService;
    private readonly IEventLogService _eventLogService;
    private readonly ICourseService _courseService;
    private readonly IEnrollmentCourseService _enrollmentCourseService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public DashboardController(IEventService eventService, IEventLogService eventLogService,
                UserManager<ApplicationUser> userManager, ICourseService courseService, IEnrollmentCourseService enrollmentCourseService, IMapper mapper)
    {
        _eventService = eventService;
        _eventLogService = eventLogService;
        _courseService = courseService;
        _enrollmentCourseService = enrollmentCourseService;
        _userManager = userManager;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var userId = _userManager.FindByNameAsync(User.Identity.Name).Result.Id;
            var courseIdList = _enrollmentCourseService.GetListByUserId(Guid.Parse(userId)).Select(a => a.CourseId).ToList();

            var events = await _eventService.GetAllAsync();

            var today = _eventLogService.FilterMapEvents(events, DateTime.Today.Date, Guid.Parse(userId), courseIdList);
            var nextDay = _eventLogService.FilterMapEvents(events, DateTime.Today.Date.AddDays(1), Guid.Parse(userId), courseIdList);

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
            return Json(new { success = true, message = $"Ocorreu um erro ao buscar os eventos {ex}", type = "error" });
        }
    }

    [HttpPost]
    public void CreateEventLog(Guid eventId, bool isChecked)
    {
        var userId = _userManager.FindByNameAsync(User.Identity.Name).Result.Id;

        _eventLogService.ToggleEventLog(eventId, Guid.Parse(userId), isChecked);
    }
}
