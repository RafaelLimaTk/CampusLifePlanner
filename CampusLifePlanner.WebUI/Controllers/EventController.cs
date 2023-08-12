using AutoMapper;
using CampusLifePlanner.Application.DTOs;
using CampusLifePlanner.Application.Interfaces;
using CampusLifePlanner.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusLifePlanner.WebUI.Controllers
{
    [Authorize]
    public class EventController : Controller
    {
        private readonly IEventService _eventService;
        private readonly IMapper _mapper;

        public EventController(IEventService eventService, IMapper mapper)
        {
            _eventService = eventService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var events = await _eventService.GetAllAsync();
            var eventDtos = _mapper.Map<IEnumerable<EventDto>>(events);
            return Json(eventDtos);
        }

        [HttpPost]
        public async Task<IActionResult> Create(EventDto eventDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _eventService.CreateAsync(eventDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro interno ao tentar criar o evento.");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(Guid id, EventDto eventDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingEvent = await _eventService.GetByIdAsync(id);
            if (existingEvent == null)
            {
                return NotFound($"Evento com ID {id} não encontrado.");
            }

            _mapper.Map(eventDto, existingEvent);

            try
            {
                await _eventService.UpdateAsync(existingEvent);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro interno ao tentar atualizar o evento.");
            }
        }
    }
}
