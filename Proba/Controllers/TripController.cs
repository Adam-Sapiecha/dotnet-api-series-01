
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proba.Data;
using Proba.DTOs;
using Proba.Models;
using Proba.Services;

namespace Proba.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripController : ControllerBase
    {
        private readonly ITripService _tripService;
        public TripController(ITripService tripService) => _tripService = tripService;

        [HttpGet]
        public async Task<ActionResult<TripsResponse>> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
            => Ok(await _tripService.GetPagedTripsAsync(page, pageSize));

        [HttpGet("{idTrip}")]
        public async Task<ActionResult<TripDto>> Get(int idTrip)
        {
            var dto = await _tripService.GetTripByIdAsync(idTrip);
            return dto == null ? NotFound() : Ok(dto);
        }

        [HttpPost("{idTrip}/clients")]
        public async Task<IActionResult> PostClient(int idTrip, [FromBody] ClientCreateDto dto)
        {
            try
            {
                var added = await _tripService.AddClientToTripAsync(idTrip, dto);
                if (!added) return BadRequest("Cannot register for this trip.");
                return CreatedAtAction(nameof(Get), new { idTrip }, null);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}