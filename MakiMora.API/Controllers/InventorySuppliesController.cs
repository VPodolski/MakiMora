using Microsoft.AspNetCore.Mvc;
using MakiMora.Core.DTOs;
using MakiMora.Core.Services;
using Microsoft.AspNetCore.Authorization;

namespace MakiMora.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventorySuppliesController : ControllerBase
    {
        private readonly IInventorySupplyService _inventorySupplyService;

        public InventorySuppliesController(IInventorySupplyService inventorySupplyService)
        {
            _inventorySupplyService = inventorySupplyService;
        }

        [HttpGet]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<IEnumerable<InventorySupplyDto>>> GetSupplies([FromQuery] Guid? locationId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            IEnumerable<InventorySupplyDto> supplies;

            if (startDate.HasValue && endDate.HasValue)
            {
                supplies = await _inventorySupplyService.GetSuppliesByDateRangeAsync(startDate.Value, endDate.Value);
            }
            else if (locationId.HasValue)
            {
                supplies = await _inventorySupplyService.GetSuppliesByLocationAsync(locationId.Value);
            }
            else
            {
                supplies = await _inventorySupplyService.GetSuppliesAsync();
            }

            return Ok(supplies);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<InventorySupplyDto>> GetSupply(Guid id)
        {
            var supply = await _inventorySupplyService.GetSupplyByIdAsync(id);
            if (supply == null)
            {
                return NotFound();
            }

            return Ok(supply);
        }

        [HttpPost]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<InventorySupplyDto>> CreateSupply([FromBody] CreateInventorySupplyRequestDto createSupplyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var supply = await _inventorySupplyService.CreateSupplyAsync(createSupplyDto);
                return CreatedAtAction(nameof(GetSupply), new { id = supply.Id }, supply);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<InventorySupplyDto>> UpdateSupply(Guid id, [FromBody] UpdateInventorySupplyRequestDto updateSupplyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var supply = await _inventorySupplyService.UpdateSupplyAsync(id, updateSupplyDto);
                return Ok(supply);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> DeleteSupply(Guid id)
        {
            var result = await _inventorySupplyService.DeleteSupplyAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPatch("{id}/status")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<InventorySupplyDto>> UpdateSupplyStatus(Guid id, [FromBody] UpdateSupplyStatusRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var supply = await _inventorySupplyService.UpdateSupplyStatusAsync(id, request.Status);
                return Ok(supply);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("by-manager/{managerId}")]
        [Authorize(Roles = "manager,hr")]
        public async Task<ActionResult<IEnumerable<InventorySupplyDto>>> GetSuppliesByManager(Guid managerId)
        {
            var supplies = await _inventorySupplyService.GetSuppliesByManagerAsync(managerId);
            return Ok(supplies);
        }
    }
}