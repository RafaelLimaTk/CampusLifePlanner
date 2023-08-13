using AutoMapper;
using CampusLifePlanner.Application.DTOs;
using CampusLifePlanner.Application.Interfaces;
using CampusLifePlanner.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.MSIdentity.Shared;
using RS = CampusLifePlanner.Infra.IoC.Resources;

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
    public async Task<JsonResult> Create(CourseDto courseDto)
    {
        try
        {
            await _courseService.CreateAsync(courseDto);
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = RS.Common.EX_MSG_CREATE_ERROR.Replace("{0}", RS.Common.GENERAL_PAGE_LBL_COURSE), type = "info" });
        }
    }

    public async Task<ActionResult> Edit(Guid id)
        => PartialView("Modal_Create_Course", _mapper.Map<CourseDto>(await _courseService.GetByIdAsync(id)));

    [HttpPost]
    public async Task<JsonResult> Delete(Guid id)
    {
        try
        {
            if (_courseService.ExistEvent(id))
                throw new Exception("Existem eventos cadastrados para esse curso");

            await _courseService.DeleteAsync(id);
            TempData["success"] = "Sucesso";
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message, type = "error" });
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
        catch (Exception ex)
        {
            return Json(new { success = false, message = RS.Common.EX_MSG_CREATE_ERROR.Replace("{0}", RS.Common.GENERAL_PAGE_LBL_COURSE), type = "info" });
        }

    }

    public ActionResult ModalDelete()
            => PartialView("Modal_Delete");

    public ActionResult Modal_Create_Course()
    {
        return PartialView(new CourseDto());
    }
}
