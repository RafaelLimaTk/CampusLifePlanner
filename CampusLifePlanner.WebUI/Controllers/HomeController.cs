using CampusLifePlanner.Application.DTOs;
using CampusLifePlanner.Application.Interfaces;
using CampusLifePlanner.Domain.Entities;
using CampusLifePlanner.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;


namespace CampusLifePlanner.WebUI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IEnrollmentCourseService _enrollmentCourseService;
        private readonly IEventService _eventService;

        public HomeController(ICourseService courseService, IEnrollmentCourseService enrollmentCourseService, IEventService eventService)
        {
            _courseService = courseService;
            _enrollmentCourseService = enrollmentCourseService;
            _eventService = eventService;
        }

        #region GetList
        private async Task<IEnumerable<Course>> GetCourses()
        {
            return await _courseService.GetAllAsync();
        }
        #endregion

        #region CRUD
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RelatedEnrollmentCourse()
        {
            ViewBag.Courses = new SelectList(GetCourses().Result.ToList(), "Id", "Name");
            return PartialView();
        }

        public IActionResult SelectCourse()
        {
            ViewBag.Courses = new SelectList(GetCourses().Result.ToList(), "Id", "Name");
            return PartialView();
        }

        public IActionResult CalendarStudent(Guid UserId)
        {
            var courseIdList = _enrollmentCourseService.GetListByUserId(UserId).Select(a => a.CourseId).ToList();
            ViewBag.Courses = new SelectList(_courseService.GetCourseListByCourseId(courseIdList), "Id", "Name");
            return PartialView(UserId);
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
                //model.Id = Guid.NewGuid();
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
    }
}