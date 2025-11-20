using MakiMora.Core.Entities;
using MakiMora.Core.DTOs.Auth;

namespace MakiMora.Core.Services
{
    public interface IUserService
    {
        Task<UserDto?> GetUserByIdAsync(Guid id);
        Task<UserDto?> GetUserByUsernameAsync(string username);
        Task<UserDto?> GetUserByEmailAsync(string email);
        Task<IEnumerable<UserDto>> GetUsersAsync();
        Task<UserDto> CreateUserAsync(CreateUserRequestDto createUserDto);
        Task<UserDto> UpdateUserAsync(Guid id, UpdateUserRequestDto updateUserDto);
        Task<bool> DeleteUserAsync(Guid id);
        Task<bool> AssignRoleToUserAsync(Guid userId, Guid roleId);
        Task<bool> RemoveRoleFromUserAsync(Guid userId, Guid roleId);
        Task<bool> AssignLocationToUserAsync(Guid userId, Guid locationId);
        Task<bool> RemoveLocationFromUserAsync(Guid userId, Guid locationId);
        Task<UserDto> ChangeUserPasswordAsync(Guid id, string currentPassword, string newPassword);
        Task<bool> ValidateCredentialsAsync(string username, string password);
    }
}