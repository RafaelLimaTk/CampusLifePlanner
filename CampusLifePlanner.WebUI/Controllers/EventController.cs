using AutoMapper;
using CampusLifePlanner.Application.DTOs;
using CampusLifePlanner.Application.Interfaces;
using CampusLifePlanner.Application.Interfaces.Base;
using CampusLifePlanner.Domain.Entities;
using CampusLifePlanner.WebUI.ViewModels;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RS = CampusLifePlanner.Infra.IoC.Resources.Common;

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
    public async Task<IActionResult> Modal_Create(DateTime start)
    {
        var EventCreateModel = new EventEditViewModel
        {
            Event = new EventDto()
            {
                StartDate = start.Add(DateTime.Now.TimeOfDay)
            },
            Courses = await GetCoursesSelectList()
        };

        return PartialView(EventCreateModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(EventEditViewModel eventDto)
    {
        ModelState.Remove("Courses");
        ModelState.Remove("Event.Courses");
        string jobId = "-";
        if (!ModelState.IsValid)
        {
            return ErrorResponse(RS.EX_MSG_MODEL_INVALID);
        }

        try
        {
            ValidateDate(eventDto.Event.StartDate, eventDto.Event.EndDate);
            eventDto.Event.Id = Guid.NewGuid();
            var timeUntilDeletion = eventDto.Event.EndDate.AddDays(1) - DateTime.Now;
            jobId = BackgroundJob.Schedule(() => _eventService.DeleteAsync(eventDto.Event.Id), timeUntilDeletion);
            eventDto.Event.JobId = jobId;

            await _eventService.CreateAsync(eventDto.Event);

            TempData["success"] = RS.GENERAL_PAGE_MSG__CREATED_SUCCESS.Replace("{0}", RS.GENERAL_PAGE_LBL_EVENT);
            return SuccessResponse(RS.GENERAL_PAGE_MSG__CREATED_SUCCESS.Replace("{0}", RS.GENERAL_PAGE_LBL_EVENT));
        }
        catch (Exception ex)
        {
            RemoveJobById(jobId);
            return ErrorResponse(ex.Message);
        }
    }

    private void ValidateDate(DateTime start, DateTime end)
    {
        if (start > end)
            throw new Exception(RS.EX_MSG_DATE_START_END_INVALID);
    }

    [HttpPost]
    public async Task<JsonResult> Update(Guid id, EventEditViewModel eventDto)
    {
        ModelState.Remove("Courses");
        ModelState.Remove("Event.Courses");
        string jobId = "-";
        string jobIdSource = eventDto.Event.JobId;
        if (!ModelState.IsValid)
        {
            return ErrorResponse(RS.EX_MSG_MODEL_INVALID);
        }

        await _eventService.BeginTransaction();
        try
        {
            ValidateDate(eventDto.Event.StartDate, eventDto.Event.EndDate);
            var timeUntilDeletion = eventDto.Event.EndDate.AddDays(1) - DateTime.Now;
            jobId = BackgroundJob.Schedule(() => _eventService.DeleteAsync(eventDto.Event.Id), timeUntilDeletion);
            eventDto.Event.JobId = jobId;
            await _eventService.UpdateAsync(_mapper.Map<Event>(eventDto.Event));
            RemoveJobById(jobIdSource);

            await _eventService.CommitTransaction();
            TempData["success"] = RS.GENERAL_PAGE_MSG_UPDATE_SUCCESS.Replace("{0}", RS.GENERAL_PAGE_LBL_EVENT);
            return SuccessResponse(RS.GENERAL_PAGE_MSG_UPDATE_SUCCESS.Replace("{0}", RS.GENERAL_PAGE_LBL_EVENT));
        }
        catch (Exception ex)
        {
            RemoveJobById(jobId);
            await _eventService.RollbackTransaction();
            return ErrorResponse(ex.Message);
        }
    }

    private void RemoveJobById(string jobId)
        => BackgroundJob.Delete(jobId);

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
                throw new Exception(RS.EX_MSG_CANNOT_BE_NULL.Replace("{0}", RS.GENERAL_PRT_LBL_ID));

            await _eventService.DeleteAsync(id);

            TempData["success"] = RS.GENERAL_PAGE_MSG_DELETE_SUCCESS.Replace("{0}", RS.GENERAL_PAGE_LBL_EVENT);
            return SuccessResponse(RS.GENERAL_PAGE_MSG_DELETE_SUCCESS.Replace("{0}", RS.GENERAL_PAGE_LBL_EVENT));
        }
        catch (Exception ex)
        {
            return ErrorResponse(ex.Message);
        }
    }
    #endregion

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

