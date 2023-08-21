using CampusLifePlanner.Application.Interfaces;
using CampusLifePlanner.Application.Services;
using CampusLifePlanner.Domain.Account;
using CampusLifePlanner.Domain.Entities;
using CampusLifePlanner.WebUI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RS = CampusLifePlanner.Infra.IoC.Resources;

namespace CampusLifePlanner.WebUI.Controllers;

public class AccountController : Controller
{
    private readonly ICourseService _courseService;
    private readonly IEnrollmentCourseService _enrollmentCourseService;
    private readonly IAuthenticate _authentication;

    public AccountController(ICourseService courseService, IEnrollmentCourseService enrollmentCourseService, IAuthenticate authentication)
    {
        _courseService = courseService;
        _enrollmentCourseService = enrollmentCourseService;
        _authentication = authentication;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _authentication.Authentication(model.Email, model.Password);

            if (result)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["warning"] = RS.Common.EX_MSG_INVALID_USER.Replace("{0}", RS.Common.GENERAL_PAGE_LBL_USER).Replace("{1}", RS.Common.GENERAL_PAGE_LBL_PASSWORD);
            }
        }
        return View(model);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _authentication.RegisterUser(model.FirstName, model.LastName, model.Email, model.Password);

            if (result.success)
            {
                return Redirect("/");
            }
            else
            {
                TempData["info"] = result.msg;
            }
        }
        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await _authentication.Logout();
        return Redirect("/Account/Login");
    }

    [HttpGet]
    public IActionResult Profile()
    {
        return View();
    }

    [HttpGet]
    public JsonResult GetCoursesByUserId(Guid userId)
    {
        var courseIdList = _enrollmentCourseService.GetListByUserId(userId).Select(a => a.CourseId).ToList();
        var courses = _courseService.GetCourseListByCourseId(courseIdList)
            .Select(c => new
            {
                Id = c.Id,
                Name = c.Name,
            }).ToList();

        return Json(courses);
    }

    private async Task<IEnumerable<Course>> GetCourses()
    {
        return await _courseService.GetAllAsync();
    }
}
