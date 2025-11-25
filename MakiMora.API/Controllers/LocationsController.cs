using Microsoft.AspNetCore.Mvc;
using MakiMora.Core.DTOs;
using MakiMora.Core.Services;
using Microsoft.AspNetCore.Authorization;

namespace MakiMora.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationsController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationsController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<LocationDto>>> GetLocations()
        {
            var locations = await _locationService.GetLocationsAsync();
            return Ok(locations);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<LocationDto>> GetLocation(Guid id)
        {
            var location = await _locationService.GetLocationByIdAsync(id);
            if (location == null)
            {
                return NotFound();
            }

            return Ok(location);
        }

        [HttpPost]
        [Authorize(Roles = "hr")]
        public async Task<ActionResult<LocationDto>> CreateLocation([FromBody] CreateLocationRequestDto createLocationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var location = await _locationService.CreateLocationAsync(createLocationDto);
                return CreatedAtAction(nameof(GetLocation), new { id = location.Id }, location);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "hr")]
        public async Task<ActionResult<LocationDto>> UpdateLocation(Guid id, [FromBody] UpdateLocationRequestDto updateLocationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var location = await _locationService.UpdateLocationAsync(id, updateLocationDto);
                return Ok(location);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "hr")]
        public async Task<IActionResult> DeleteLocation(Guid id)
        {
            var result = await _locationService.DeleteLocationAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("with-users")]
        [Authorize(Roles = "manager,hr")]
        public async Task<ActionResult<IEnumerable<LocationDto>>> GetLocationsWithUsers()
        {
            var locations = await _locationService.GetLocationsWithUsersAsync();
            return Ok(locations);
        }

        [HttpGet("by-manager/{managerId}")]
        [Authorize(Roles = "hr")]
        public async Task<ActionResult<IEnumerable<LocationDto>>> GetLocationsByManager(Guid managerId)
        {
            var locations = await _locationService.GetLocationsByManagerAsync(managerId);
            return Ok(locations);
        }
    }
}