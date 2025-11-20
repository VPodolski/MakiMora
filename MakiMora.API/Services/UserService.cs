using AutoMapper;
using MakiMora.Core.Entities;
using MakiMora.Core.Repositories;
using MakiMora.Core.Services;
using MakiMora.Core.DTOs.Auth;
using Microsoft.EntityFrameworkCore;

namespace MakiMora.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;

        public UserService(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            ILocationRepository locationRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _locationRepository = locationRepository;
            _mapper = mapper;
        }

        public async Task<UserDto?> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> GetUserByUsernameAsync(string username)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null) return null;

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null) return null;

            return _mapper.Map<UserDto>(user);
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(u => _mapper.Map<UserDto>(u));
        }

        public async Task<UserDto> CreateUserAsync(CreateUserRequestDto createUserDto)
        {
            // Check if user with this username or email already exists
            if (await _userRepository.ExistsByUsernameAsync(createUserDto.Username))
                throw new ArgumentException($"User with username '{createUserDto.Username}' already exists");
            
            if (await _userRepository.ExistsByEmailAsync(createUserDto.Email))
                throw new ArgumentException($"User with email '{createUserDto.Email}' already exists");

            var user = new User
            {
                Username = createUserDto.Username,
                Email = createUserDto.Email,
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                Phone = createUserDto.Phone,
                IsActive = createUserDto.IsActive
            };

            // Hash password (in real implementation)
            user.PasswordHash = HashPassword(createUserDto.Password);

            var createdUser = await _userRepository.AddAsync(user);

            // Assign roles if provided
            if (createUserDto.RoleIds != null && createUserDto.RoleIds.Any())
            {
                foreach (var roleId in createUserDto.RoleIds)
                {
                    var role = await _roleRepository.GetByIdAsync(roleId);
                    if (role != null)
                    {
                        var userRole = new UserRole
                        {
                            UserId = createdUser.Id,
                            RoleId = roleId
                        };
                        // Assuming there's a way to add user roles through repository or context
                    }
                }
            }

            // Assign locations if provided
            if (createUserDto.LocationIds != null && createUserDto.LocationIds.Any())
            {
                foreach (var locationId in createUserDto.LocationIds)
                {
                    var location = await _locationRepository.GetByIdAsync(locationId);
                    if (location != null)
                    {
                        var userLocation = new UserLocation
                        {
                            UserId = createdUser.Id,
                            LocationId = locationId
                        };
                        // Assuming there's a way to add user locations through repository or context
                    }
                }
            }

            return _mapper.Map<UserDto>(createdUser);
        }

        public async Task<UserDto> UpdateUserAsync(Guid id, UpdateUserRequestDto updateUserDto)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null)
                throw new ArgumentException($"User with id '{id}' not found");

            existingUser.Username = updateUserDto.Username;
            existingUser.Email = updateUserDto.Email;
            existingUser.FirstName = updateUserDto.FirstName;
            existingUser.LastName = updateUserDto.LastName;
            existingUser.Phone = updateUserDto.Phone;
            existingUser.IsActive = updateUserDto.IsActive;

            var updatedUser = await _userRepository.UpdateAsync(existingUser);
            return _mapper.Map<UserDto>(updatedUser);
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return false;

            await _userRepository.DeleteAsync(user);
            return true;
        }

        public async Task<bool> AssignRoleToUserAsync(Guid userId, Guid roleId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            var role = await _roleRepository.GetByIdAsync(roleId);

            if (user == null || role == null) return false;

            var userRole = new UserRole
            {
                UserId = userId,
                RoleId = roleId
            };

            // Assuming there's a way to add user roles through repository or context
            return true;
        }

        public async Task<bool> RemoveRoleFromUserAsync(Guid userId, Guid roleId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            var role = await _roleRepository.GetByIdAsync(roleId);

            if (user == null || role == null) return false;

            // Assuming there's a way to remove user roles through repository or context
            return true;
        }

        public async Task<bool> AssignLocationToUserAsync(Guid userId, Guid locationId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            var location = await _locationRepository.GetByIdAsync(locationId);

            if (user == null || location == null) return false;

            var userLocation = new UserLocation
            {
                UserId = userId,
                LocationId = locationId
            };

            // Assuming there's a way to add user locations through repository or context
            return true;
        }

        public async Task<bool> RemoveLocationFromUserAsync(Guid userId, Guid locationId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            var location = await _locationRepository.GetByIdAsync(locationId);

            if (user == null || location == null) return false;

            // Assuming there's a way to remove user locations through repository or context
            return true;
        }

        public async Task<UserDto> ChangeUserPasswordAsync(Guid id, string currentPassword, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new ArgumentException($"User with id '{id}' not found");

            // Verify current password (in real implementation)
            if (!VerifyPassword(currentPassword, user.PasswordHash))
                throw new ArgumentException("Current password is incorrect");

            user.PasswordHash = HashPassword(newPassword);
            var updatedUser = await _userRepository.UpdateAsync(user);

            return _mapper.Map<UserDto>(updatedUser);
        }

        public async Task<bool> ValidateCredentialsAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null) return false;

            return VerifyPassword(password, user.PasswordHash);
        }

        private string HashPassword(string password)
        {
            // In a real implementation, use a proper password hashing library
            // For example: BCrypt.Net or Microsoft.AspNetCore.Cryptography.KeyDerivation
            return password; // Placeholder
        }

        private bool VerifyPassword(string password, string hash)
        {
            // In a real implementation, verify the password against the hash
            return password == hash; // Placeholder
        }
    }
}