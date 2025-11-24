using Microsoft.AspNetCore.Mvc;
using MakiMora.Core.DTOs;
using MakiMora.Core.Services;
using Microsoft.AspNetCore.Authorization;

namespace MakiMora.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "manager")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductsController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts([FromQuery] Guid? locationId, [FromQuery] Guid? categoryId)
        {
            IEnumerable<ProductDto> products;

            if (locationId.HasValue && categoryId.HasValue)
            {
                products = await _productService.GetProductsByLocationAndCategoryAsync(locationId.Value, categoryId.Value);
            }
            else if (locationId.HasValue)
            {
                products = await _productService.GetProductsByLocationAsync(locationId.Value);
            }
            else if (categoryId.HasValue)
            {
                products = await _productService.GetProductsByCategoryAsync(categoryId.Value);
            }
            else
            {
                products = await _productService.GetProductsAsync();
            }

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] Core.DTOs.CreateProductRequestDto createProductDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var product = await _productService.CreateProductAsync(createProductDto);
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDto>> UpdateProduct(Guid id, [FromBody] Core.DTOs.UpdateProductRequestDto updateProductDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var product = await _productService.UpdateProductAsync(id, updateProductDto);
                return Ok(product);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPatch("{id}/availability")]
        public async Task<ActionResult<ProductDto>> UpdateProductAvailability(Guid id, [FromBody] UpdateProductAvailabilityRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var product = await _productService.UpdateProductAvailabilityAsync(id, request.IsAvailable);
                return Ok(product);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPatch("{id}/stop-list")]
        public async Task<ActionResult<ProductDto>> UpdateProductStopListStatus(Guid id, [FromBody] UpdateProductStopListStatusRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var product = await _productService.UpdateProductStopListStatusAsync(id, request.IsOnStopList);
                return Ok(product);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAvailableProducts([FromQuery] Guid? locationId)
        {
            IEnumerable<ProductDto> products;

            if (locationId.HasValue)
            {
                products = await _productService.GetProductsByLocationAsync(locationId.Value);
                products = products.Where(p => p.IsAvailable && !p.IsOnStopList);
            }
            else
            {
                products = await _productService.GetAvailableProductsAsync();
            }

            return Ok(products);
        }

        [HttpGet("stop-list")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetStopListProducts([FromQuery] Guid? locationId)
        {
            IEnumerable<ProductDto> products;

            if (locationId.HasValue)
            {
                products = await _productService.GetProductsByLocationAsync(locationId.Value);
                products = products.Where(p => p.IsOnStopList);
            }
            else
            {
                products = await _productService.GetOnStopListProductsAsync();
            }

            return Ok(products);
        }
    }

    public class UpdateProductAvailabilityRequestDto
    {
        public bool IsAvailable { get; set; }
    }

    public class UpdateProductStopListStatusRequestDto
    {
        public bool IsOnStopList { get; set; }
    }
}