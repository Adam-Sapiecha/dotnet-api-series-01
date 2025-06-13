using Microsoft.AspNetCore.Mvc;
using Proba.DTOs;
using Proba.Services;


namespace Proba.Controllers;



[ApiController]
[Route("api/events")]
public class EventsController : ControllerBase
{
    private readonly IEventService _eventService;
    public EventsController(IEventService eventService) => _eventService = eventService;


    [HttpGet("details")]
    public async Task<ActionResult<List<EventDetailDto>>> GetDetails()
    {
        var details = await _eventService.GetAllEventDetailsAsync();
        return Ok(details);
    }


    [HttpPut("{id}")]
    public async Task<ActionResult<EditEventResultDto>> Update(int id, [FromBody] EditEventDto dto)
    {
        try
        {
            var result = await _eventService.UpdateEventAsync(id, dto);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}