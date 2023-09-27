using AutoMapper;
using CampusLifePlanner.Application.DTOs;
using CampusLifePlanner.Application.Interfaces;
using CampusLifePlanner.Domain.Entities;
using CampusLifePlanner.Infra.Data.Identity;
using CampusLifePlanner.WebUI.Models;
using CampusLifePlanner.WebUI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;


namespace CampusLifePlanner.WebUI.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly IMapper _mapper;
    private readonly ICourseService _courseService;
    private readonly IEnrollmentCourseService _enrollmentCourseService;
    private readonly IEventService _eventService;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(IMapper mapper, ICourseService courseService, 
        IEnrollmentCourseService enrollmentCourseService, 
        IEventService eventService, 
        UserManager<ApplicationUser> userManager)
    {
        _mapper = mapper;
        _courseService = courseService;
        _enrollmentCourseService = enrollmentCourseService;
        _eventService = eventService;
        _userManager = userManager;
    }

    #region CRUD
    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> RelatedEnrollmentCourse()
    {
        var user = await _userManager.FindByNameAsync(User.Identity.Name);
        var userId = user.Id;

        var enrollmentCourse = await _courseService.GetCoursesUserIsNotEnrolledIn(Guid.Parse(userId));
        var CoursesModel = new CoursesViewModel
        {
            Courses = new SelectList(enrollmentCourse.ToList(), "Id", "Name")
        };

        return PartialView(CoursesModel);
    }

    public async Task<IActionResult> SelectCourse()
    {
        var CoursesModel = new CoursesViewModel
        {
            Courses = await GetCoursesSelectList()
        };

        return PartialView(CoursesModel);
    }

    public IActionResult CalendarStudent(Guid UserId)
    {
        var courseIdList = _enrollmentCourseService.GetListByUserId(UserId).Select(a => a.CourseId).ToList();
        ViewBag.Courses = new SelectList(_courseService.GetCourseListByCourseId(courseIdList), "Id", "Name");
        return PartialView(UserId);
    }

    public async Task<IActionResult> Modal_Detail(Guid id)
    {
        var result = _mapper.Map<EventDto>(await _eventService.GetWithCourseById(id));
        return PartialView(result);
    }

    public JsonResult GetEventsByCourseId(Guid courseId)
    {
        var events = _eventService.GetAllByCourseIdAsync(courseId).Result;
        return Json(events);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    #endregion

    #region EnrollmentCourese
    public bool HasEnrollmentCourseByUserId(Guid id)
    {
        var enrollmentCourse = _enrollmentCourseService.HasEnrollmentCourseByUserId(id);
        if (enrollmentCourse)
            return true;
        else
            return false;
    }

    [HttpPost]
    public JsonResult EnrollmentCourse_Save(EnrollmentCourseDto model)
    {
        try
        {
            _enrollmentCourseService.CreateAsync(model);
            return Json(new
            {
                success = true,
            });
        }
        catch (Exception ex)
        {
            return Json(new
            {
                success = false,
                msg = ex.Message
            });
        }

    }

    #endregion

    #region GetListCourse
    private async Task<IEnumerable<Course>> GetCourses()
    {
        return await _courseService.GetAllAsync();
    }
    
    private async Task<SelectList> GetCoursesSelectList()
    {
        var courses = await GetCourses();
        return new SelectList(courses.ToList(), "Id", "Name");
    }
    #endregion
}