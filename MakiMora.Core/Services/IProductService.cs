using MakiMora.Core.Entities;

namespace MakiMora.Core.Services
{
    public interface IProductService
    {
        Task<ProductDto?> GetProductByIdAsync(Guid id);
        Task<IEnumerable<ProductDto>> GetProductsAsync();
        Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(Guid categoryId);
        Task<IEnumerable<ProductDto>> GetProductsByLocationAsync(Guid locationId);
        Task<IEnumerable<ProductDto>> GetAvailableProductsAsync();
        Task<ProductDto> CreateProductAsync(CreateProductRequestDto createProductDto);
        Task<ProductDto> UpdateProductAsync(Guid id, UpdateProductRequestDto updateProductDto);
        Task<bool> DeleteProductAsync(Guid id);
        Task<ProductDto> UpdateProductAvailabilityAsync(Guid id, bool isAvailable);
        Task<ProductDto> UpdateProductStopListStatusAsync(Guid id, bool isOnStopList);
        Task<IEnumerable<ProductDto>> GetProductsByLocationAndCategoryAsync(Guid locationId, Guid categoryId);
        Task<IEnumerable<ProductDto>> GetOnStopListProductsAsync();
    }
}