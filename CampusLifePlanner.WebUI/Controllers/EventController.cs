using AutoMapper;
using CampusLifePlanner.Application.DTOs;
using CampusLifePlanner.Application.Interfaces;
using CampusLifePlanner.Domain.Entities;
using CampusLifePlanner.WebUI.ViewModels;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CampusLifePlanner.WebUI.Controllers;

[Authorize]
public class EventController : Controller
{
    private readonly IEventService _eventService;
    private readonly IMapper _mapper;
    private readonly ICourseService _courseService;

    public EventController(IEventService eventService,
                           IMapper mapper,
                           ICourseService courseService)
    {
        _eventService = eventService;
        _mapper = mapper;
        _courseService = courseService;
    }

    #region CRUD
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Calendar()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetEvents()
    {
        var events = await _eventService.GetAllAsync();
        var eventDtos = _mapper.Map<IEnumerable<EventDto>>(events);
        return Json(eventDtos);
    }

    [HttpGet]
    public async Task<IActionResult> Modal_Edit(Guid id)
    {
        var eventEntity = await _eventService.GetByIdAsync(id);

        var EventEditModel = new EventEditViewModel
        {
            Event = _mapper.Map<EventDto>(eventEntity),
            Courses = await GetCoursesSelectList()
        };

        return PartialView("Modal_Create", EventEditModel);
    }

    [HttpGet]
    public async Task<IActionResult> Modal_Create()
    {
        var EventCreateModel = new EventEditViewModel
        {
            Event = new EventDto(),
            Courses = await GetCoursesSelectList()
        };

        return PartialView(EventCreateModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(EventEditViewModel eventDto)
    {
        ModelState.Remove("Courses");

        if (!ModelState.IsValid)
        {
            return ErrorResponse("Modelo não é válido");
        }

        try
        {
            eventDto.Event.Id = Guid.NewGuid();
            await _eventService.CreateAsync(eventDto.Event);

            var timeUntilDeletion = eventDto.Event.EndDate.AddDays(1) - DateTime.Now;

            BackgroundJob.Schedule(() => _eventService.DeleteAsync(eventDto.Event.Id), timeUntilDeletion);

            TempData["success"] = "Evento criado com sucesso";
            return SuccessResponse("Evento criado com sucesso");
        }
        catch (Exception ex)
        {
            return ErrorResponse(ex.Message);
        }
    }

    [HttpPost]
    public async Task<JsonResult> Update(Guid id, EventEditViewModel eventDto)
    {
        ModelState.Remove("Courses");

        if (!ModelState.IsValid)
        {
            return ErrorResponse("Modelo não é válido");
        }

        try
        {
            await _eventService.UpdateAsync(_mapper.Map<Event>(eventDto.Event));
            TempData["success"] = "Evento atualizado com sucesso";
            return SuccessResponse("Evento atualizado com sucesso");
        }
        catch (Exception ex)
        {
            return ErrorResponse(ex.Message);
        }
    }

    [HttpPost]
    public ActionResult ModalDelete()
        => PartialView("Modal_Delete");

    [HttpGet]
    public async Task<ActionResult> SharedEvent(Guid id)
    {
        var courses = await GetCourses();
        var eventEntity = await _eventService.GetByIdAsync(id);

        var sharedEventViewModel = new SharedEventViewModel
        {
            Event = _mapper.Map<EventDto>(eventEntity),
            Courses = new SelectList(courses.ToList(), "Id", "Name")
        };

        return View(sharedEventViewModel);
    }

    [HttpPost]
    public async Task<JsonResult> Delete(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
                throw new Exception("Id não pode ser null");

            await _eventService.DeleteAsync(id);
            TempData["success"] = "Evento deletado com sucesso";
            return SuccessResponse("Evento deletado com sucesso");
        }
        catch (Exception ex)
        {
            return ErrorResponse(ex.Message);
        }
    }

    #region Metodos de tratação de erro
    private JsonResult SuccessResponse(string message)
    {
        return Json(new { success = true, message = message, type = "success" });
    }

    private JsonResult ErrorResponse(string message, string type = "error")
    {
        return Json(new { success = false, message = message, type = type });
    }
    #endregion

    #region GetList
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
#endregion
