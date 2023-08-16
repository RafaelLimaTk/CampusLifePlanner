using AutoMapper;
using CampusLifePlanner.Application.DTOs;
using CampusLifePlanner.Application.Interfaces;
using CampusLifePlanner.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            var courses = await GetCourses();
            ViewBag.Courses = new SelectList(courses.ToList(), "Id", "Name");
            var eventEntity = await _eventService.GetByIdAsync(id);
            return PartialView("Modal_Create", _mapper.Map<EventDto>(eventEntity));
        }

        [HttpGet]
        public async Task<IActionResult> Modal_Create()
        {
            var courses = await GetCourses();
            ViewBag.Courses = new SelectList(courses.ToList(), "Id", "Name");
            return PartialView(new EventDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create(EventDto eventDto)
        {

            if (!ModelState.IsValid)
            {
                return ErrorResponse("Modelo não é válido");
            }

            try
            {
                await _eventService.CreateAsync(eventDto);
                TempData["success"] = "Evento criado com sucesso";
                return SuccessResponse("Evento criado com sucesso");
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex.Message);
            }
        }

        [HttpPost]
        public async Task<JsonResult> Update(Guid id, EventDto eventDto)
        {
            if (!ModelState.IsValid)
            {
                return ErrorResponse("Modelo não é válido");
            }

            try
            {
                await _eventService.UpdateAsync(_mapper.Map<Event>(eventDto));
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
    }
    #endregion
}
