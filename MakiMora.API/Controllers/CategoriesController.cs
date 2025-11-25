using Microsoft.AspNetCore.Mvc;
using MakiMora.Core.DTOs;
using MakiMora.Core.Services;
using Microsoft.AspNetCore.Authorization;

namespace MakiMora.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories([FromQuery] Guid? locationId)
        {
            IEnumerable<CategoryDto> categories;

            if (locationId.HasValue)
            {
                categories = await _categoryService.GetCategoriesByLocationAsync(locationId.Value);
            }
            else
            {
                categories = await _categoryService.GetCategoriesAsync();
            }

            return Ok(categories);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<CategoryDto>> GetCategory(Guid id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] CreateCategoryRequestDto createCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var category = await _categoryService.CreateCategoryAsync(createCategoryDto);
                return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<CategoryDto>> UpdateCategory(Guid id, [FromBody] UpdateCategoryRequestDto updateCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var category = await _categoryService.UpdateCategoryAsync(id, updateCategoryDto);
                return Ok(category);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("with-products")]
        [Authorize(Roles = "manager,hr")]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategoriesWithProducts([FromQuery] Guid locationId)
        {
            if (locationId == Guid.Empty)
            {
                return BadRequest(new { message = "Location ID is required" });
            }

            var categories = await _categoryService.GetCategoriesWithProductsAsync(locationId);
            return Ok(categories);
        }
    }
}