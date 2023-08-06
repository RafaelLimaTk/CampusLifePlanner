using AutoMapper;
using CampusLifePlanner.Application.DTOs;
using CampusLifePlanner.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusLifePlanner.WebUI.Controllers;

[Authorize]
public class CourseController : Controller
{
    private readonly ICourseService _courseService;
    private readonly IMapper _mapper;
    public CourseController(ICourseService courseService, IMapper mapper)
    {
        _courseService = courseService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var course = await _courseService.GetAllAsync();
        var courseDto = _mapper.Map<IEnumerable<CourseDto>>(course);

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
        if (!ModelState.IsValid)
        {
            return View(courseDto);
        }

        try
        {
            await _courseService.CreateAsync(courseDto);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, "Ocorreu um erro ao criar o curso. Tente novamente.");

            return View(courseDto);
        }

        return RedirectToAction("Index");
    }
}
