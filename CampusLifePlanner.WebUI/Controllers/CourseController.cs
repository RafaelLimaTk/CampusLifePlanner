using AutoMapper;
using CampusLifePlanner.Application.DTOs;
using CampusLifePlanner.Application.Interfaces;
using CampusLifePlanner.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using RS = CampusLifePlanner.Infra.IoC.Resources.Common;

namespace CampusLifePlanner.WebUI.Controllers;

[Authorize]
public class CourseController : Controller
{
    private readonly ICourseService _courseService;
    private readonly IEventService _eventService;
    private readonly IMapper _mapper;
    public CourseController(ICourseService courseService, IEventService eventService, IMapper mapper)
    {
        _courseService = courseService;
        _eventService = eventService;
        _mapper = mapper;
    }

    #region CRUD
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var course = await _courseService.GetAllAsync();
        var courseDto = _mapper.Map<IEnumerable<CourseDto>>(course);

        foreach (var c in courseDto)
        {
            c.Sigla = GenerateSigla(c.Name);
            c.Events = (await _eventService.GetAllByCourseIdAsync(c.Id)).ToList();
            c.EnrollmentCount = await _courseService.GetEnrollmentCountByCourseId(c.Id);
        }

        return View(courseDto);
    }

    [HttpGet]
    public async Task<IActionResult> GetCourses()
    {
        var courses = await _courseService.GetAllAsync();
        var courseDtos = _mapper.Map<IEnumerable<CourseDto>>(courses);

        return Json(courseDtos);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CourseDto courseDto)
    {
        if (courseDto.Name == null) return Json(new { success = false, message = RS.EX_MSG_NULL_ERROR, type = "info" });

        try
        {
            await _courseService.CreateAsync(courseDto);
            return Json(new { success = true });
        }
        catch (Exception)
        {
            return Json(new { success = false, message = RS.EX_MSG_CREATE_ERROR.Replace("{0}", RS.GENERAL_PAGE_LBL_COURSE), type = "info" });
        }
    }

    public async Task<ActionResult> Edit(Guid id)
        => PartialView("Modal_Create_Course", _mapper.Map<CourseDto>(await _courseService.GetByIdAsync(id)));

    [HttpPost]
    public async Task<JsonResult> Delete(Guid id)
    {
        try
        {
            if ((await _eventService.GetAllAsync()).Any(a => a.Id == id))
                throw new Exception(RS.EX_MSG_EVENTS_REGISTERED_FOR_THIS_COURSE);

            
            await _courseService.DeleteAsync(id);
            TempData["success"] = RS.GENERAL_PAGE_MSG_DELETE_SUCCESS.Replace("{0}", RS.GENERAL_PAGE_LBL_COURSE);
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = RS.EX_MSG_EVENTS_REGISTERED_FOR_THIS_COURSE, type = "error" });
        }

    }

    [HttpPost]
    public async Task<JsonResult> Update(CourseDto courseDto)
    {
        try
        {
            await _courseService.UpdateAsync(_mapper.Map<Course>(courseDto));
            TempData["success"] = "Salvo";
            return Json(new { success = true });
        }
        catch (Exception)
        {
            return Json(new { success = false, message = RS.EX_MSG_CREATE_ERROR.Replace("{0}", RS.GENERAL_PAGE_LBL_COURSE), type = "info" });
        }

    }

    public ActionResult ModalDelete()
            => PartialView("Modal_Delete");

    public ActionResult Modal_Create_Course()
    {
        return PartialView(new CourseDto());
    }

    private string GenerateSigla(string courseName)
    {
        List<string> ignoreWords = new List<string> { "da" };

        string[] words = courseName.Split(' ');

        StringBuilder sigla = new StringBuilder();
        foreach (var word in words)
        {
            if (!string.IsNullOrEmpty(word) && !ignoreWords.Contains(word.ToLower()))
            {
                sigla.Append(word[0]);
            }
        }

        return sigla.ToString().ToUpper();
    }
    #endregion
}
