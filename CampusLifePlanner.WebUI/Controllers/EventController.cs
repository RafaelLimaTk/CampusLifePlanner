using AutoMapper;
using CampusLifePlanner.Application.DTOs;
using CampusLifePlanner.Application.Interfaces;
using CampusLifePlanner.Application.Services;
using CampusLifePlanner.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RS = CampusLifePlanner.Infra.IoC.Resources;

namespace CampusLifePlanner.WebUI.Controllers
{
    [Authorize]
    public class EventController : Controller
    {
        private readonly IEventService _eventService;
        private readonly IMapper _mapper;
        private readonly ICourseService courseService;

        public EventController(IEventService eventService,
                               IMapper mapper,
                               ICourseService courseService)
        {
            _eventService = eventService;
            _mapper = mapper;
            this.courseService = courseService;
        }

        #region GetList
        private async Task<IEnumerable<Course>> GetCourses()
        {
            return await courseService.GetAllAsync();
        }
        #endregion

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
            ViewBag.Courses = new SelectList(GetCourses().Result.ToList(), "Id", "Name");
            return PartialView("Modal_Create", _mapper.Map<EventDto>(_eventService.GetByIdAsync(id).Result));
        }

        [HttpGet]
        public async Task<IActionResult> Modal_Create()
        {
            ViewBag.Courses = new SelectList(GetCourses().Result.ToList(), "Id", "Name");
            return PartialView(new EventDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create(EventDto eventDto)
        {

            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    success = false,
                    message = "Modelo não válido",
                    type = "error"
                });
            }

            try
            {
                await _eventService.CreateAsync(eventDto);
                TempData["success"] = "Sucesso";
                return Json(new
                {
                    success = true,
                    message = "Sucesso",
                    type = "success"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = true,
                    message = ex.Message,
                    type = "error"
                });
            }
        }

        [HttpPost]
        public async Task<JsonResult> Update(Guid id, EventDto eventDto)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    success = false,
                    message = "Modelo não válido",
                    type = "error"
                });
            }

            try
            {
                await _eventService.UpdateAsync(_mapper.Map<Event>(eventDto));
                TempData["success"] = "Sucesso";
                return Json(new
                {
                    success = true,
                    message = "Sucesso",
                    type = "success"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = true,
                    message = ex.Message,
                    type = "error"
                });
            }
        }

        [HttpPost]
        public ActionResult ModalDelete()
            => PartialView("Modal_Delete");
        
        [HttpPost]
        public async Task<JsonResult> Delete(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    throw new Exception("Id não pode ser null");

                await _eventService.DeleteAsync(id);
                TempData["success"] = "Sucesso";
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message, type = "error" });
            }
        }
    }
    #endregion
}
