using AutoMapper;
using MakiMora.Core.Entities;
using MakiMora.Core.Repositories;
using MakiMora.Core.Services;

namespace MakiMora.API.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;

        public LocationService(
            ILocationRepository locationRepository,
            IMapper mapper)
        {
            _locationRepository = locationRepository;
            _mapper = mapper;
        }

        public async Task<LocationDto?> GetLocationByIdAsync(Guid id)
        {
            var location = await _locationRepository.GetByIdAsync(id);
            if (location == null) return null;

            return _mapper.Map<LocationDto>(location);
        }

        public async Task<IEnumerable<LocationDto>> GetLocationsAsync()
        {
            var locations = await _locationRepository.GetAllAsync();
            return locations.Select(l => _mapper.Map<LocationDto>(l));
        }

        public async Task<IEnumerable<LocationDto>> GetLocationsByManagerAsync(Guid managerId)
        {
            var locations = await _locationRepository.GetByManagerAsync(managerId);
            return locations.Select(l => _mapper.Map<LocationDto>(l));
        }

        public async Task<LocationDto> CreateLocationAsync(CreateLocationRequestDto createLocationDto)
        {
            var location = new Location
            {
                Name = createLocationDto.Name,
                Address = createLocationDto.Address,
                Phone = createLocationDto.Phone,
                Latitude = createLocationDto.Latitude,
                Longitude = createLocationDto.Longitude
            };

            var createdLocation = await _locationRepository.AddAsync(location);
            return _mapper.Map<LocationDto>(createdLocation);
        }

        public async Task<LocationDto> UpdateLocationAsync(Guid id, UpdateLocationRequestDto updateLocationDto)
        {
            var existingLocation = await _locationRepository.GetByIdAsync(id);
            if (existingLocation == null)
                throw new ArgumentException($"Location with id '{id}' not found");

            existingLocation.Name = updateLocationDto.Name;
            existingLocation.Address = updateLocationDto.Address;
            existingLocation.Phone = updateLocationDto.Phone;
            existingLocation.Latitude = updateLocationDto.Latitude;
            existingLocation.Longitude = updateLocationDto.Longitude;
            existingLocation.UpdatedAt = DateTime.UtcNow;

            var updatedLocation = await _locationRepository.UpdateAsync(existingLocation);
            return _mapper.Map<LocationDto>(updatedLocation);
        }

        public async Task<bool> DeleteLocationAsync(Guid id)
        {
            var location = await _locationRepository.GetByIdAsync(id);
            if (location == null) return false;

            await _locationRepository.DeleteAsync(location);
            return true;
        }

        public async Task<IEnumerable<LocationDto>> GetLocationsWithUsersAsync()
        {
            var locations = await _locationRepository.GetWithUsersAsync();
            return locations.Select(l => _mapper.Map<LocationDto>(l));
        }
    }
}