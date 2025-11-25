using Microsoft.AspNetCore.Mvc;
using MakiMora.Core.DTOs;
using MakiMora.Core.Services;
using Microsoft.AspNetCore.Authorization;

namespace MakiMora.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourierEarningsController : ControllerBase
    {
        private readonly ICourierEarningService _courierEarningService;

        public CourierEarningsController(ICourierEarningService courierEarningService)
        {
            _courierEarningService = courierEarningService;
        }

        [HttpGet]
        [Authorize(Roles = "manager,hr,courier")]
        public async Task<ActionResult<IEnumerable<CourierEarningDto>>> GetEarnings([FromQuery] Guid? courierId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            IEnumerable<CourierEarningDto> earnings;

            if (courierId.HasValue && startDate.HasValue && endDate.HasValue)
            {
                earnings = await _courierEarningService.GetEarningsByCourierAndDateRangeAsync(courierId.Value, startDate.Value, endDate.Value);
            }
            else if (courierId.HasValue)
            {
                earnings = await _courierEarningService.GetEarningsByCourierAsync(courierId.Value);
            }
            else if (startDate.HasValue && endDate.HasValue)
            {
                earnings = await _courierEarningService.GetEarningsByDateRangeAsync(startDate.Value, endDate.Value);
            }
            else
            {
                earnings = await _courierEarningService.GetEarningsAsync();
            }

            return Ok(earnings);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "manager,hr,courier")]
        public async Task<ActionResult<CourierEarningDto>> GetEarning(Guid id)
        {
            var earning = await _courierEarningService.GetEarningByIdAsync(id);
            if (earning == null)
            {
                return NotFound();
            }

            return Ok(earning);
        }

        [HttpPost]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<CourierEarningDto>> CreateEarning([FromBody] CreateCourierEarningRequestDto createEarningDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var earning = await _courierEarningService.CreateEarningAsync(createEarningDto);
                return CreatedAtAction(nameof(GetEarning), new { id = earning.Id }, earning);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> DeleteEarning(Guid id)
        {
            var result = await _courierEarningService.DeleteEarningAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("total")]
        [Authorize(Roles = "manager,hr,courier")]
        public async Task<ActionResult<decimal>> GetTotalEarnings([FromQuery] Guid courierId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (courierId == Guid.Empty)
            {
                return BadRequest(new { message = "Courier ID is required" });
            }

            var total = await _courierEarningService.GetTotalEarningsByCourierAsync(courierId, startDate, endDate);
            return Ok(total);
        }

        [HttpGet("by-courier/{courierId}")]
        [Authorize(Roles = "manager,hr")]
        public async Task<ActionResult<IEnumerable<CourierEarningDto>>> GetEarningsByCourier(Guid courierId)
        {
            var earnings = await _courierEarningService.GetEarningsByCourierAsync(courierId);
            return Ok(earnings);
        }
    }
}