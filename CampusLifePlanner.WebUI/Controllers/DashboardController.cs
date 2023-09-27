using AutoMapper;
using CampusLifePlanner.Application.DTOs;
using CampusLifePlanner.Application.Interfaces;
using CampusLifePlanner.Domain.Entities;
using CampusLifePlanner.Infra.Data.Identity;
using CampusLifePlanner.WebUI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RS = CampusLifePlanner.Infra.IoC.Resources.Common;


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
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                TempData["info"] = RS.EX_MSG_USER_NOT_FOUND;
                return View();
            }

            var userId = user.Id;

            var userIdParseGuid = Guid.Parse(userId);
            var courseIdList = _enrollmentCourseService.GetListByUserId(userIdParseGuid).Select(a => a.CourseId).ToList();
            if (!courseIdList.Any())
            {
                TempData["info"] = RS.EX_MSG_USER_NOT_ENROLLMENT_COURSE;
                return View();
            }

            var events = await _eventService.GetEventsWithCoursesAsync();

            var today = _eventLogService.FilterMapEvents(events, DateTime.Today.Date, userIdParseGuid, courseIdList);
            var nextDay = _eventLogService.FilterMapEvents(events, DateTime.Today.Date.AddDays(1), userIdParseGuid, courseIdList);

            var model = new EventViewModel
            {
                TodayEvents = _mapper.Map<IEnumerable<EventDto>>(today),
                NextDayEvents = _mapper.Map<IEnumerable<EventDto>>(nextDay)
            };

            return View(model);
        }
        catch (Exception ex)
        {
            return Json(new { success = true, message =  RS.EX_MSG_AN_ERROR_OCCORRED_FETCHING_EVENT + " " + ex.Message, type = "error" });
        }
    }

    [HttpPost]
    public async Task CreateEventLog(Guid eventId, bool isChecked)
    {
        var user = await _userManager.FindByNameAsync(User.Identity.Name);
        var userId = user.Id;

        await _eventLogService.ToggleEventLog(eventId, Guid.Parse(userId), isChecked);
    }
}
