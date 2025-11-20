using MakiMora.Core.Entities;

namespace MakiMora.Core.Services
{
    public interface ICategoryService
    {
        Task<CategoryDto?> GetCategoryByIdAsync(Guid id);
        Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
        Task<IEnumerable<CategoryDto>> GetCategoriesByLocationAsync(Guid locationId);
        Task<CategoryDto> CreateCategoryAsync(CreateCategoryRequestDto createCategoryDto);
        Task<CategoryDto> UpdateCategoryAsync(Guid id, UpdateCategoryRequestDto updateCategoryDto);
        Task<bool> DeleteCategoryAsync(Guid id);
        Task<IEnumerable<CategoryDto>> GetCategoriesWithProductsAsync(Guid locationId);
    }
}