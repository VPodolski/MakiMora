using MakiMora.Core.Entities;

namespace MakiMora.Core.Services
{
    public interface ILocationService
    {
        Task<LocationDto?> GetLocationByIdAsync(Guid id);
        Task<IEnumerable<LocationDto>> GetLocationsAsync();
        Task<IEnumerable<LocationDto>> GetLocationsByManagerAsync(Guid managerId);
        Task<LocationDto> CreateLocationAsync(CreateLocationRequestDto createLocationDto);
        Task<LocationDto> UpdateLocationAsync(Guid id, UpdateLocationRequestDto updateLocationDto);
        Task<bool> DeleteLocationAsync(Guid id);
        Task<IEnumerable<LocationDto>> GetLocationsWithUsersAsync();
    }
}