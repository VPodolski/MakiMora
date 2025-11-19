using Microsoft.AspNetCore.Mvc;

namespace MakiMora.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        
        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        [HttpPost("login")]
        public IActionResult Login([FromBody] Core.DTOs.Auth.LoginRequestDto loginRequest)
        {
            // В реальном приложении здесь должна быть логика аутентификации
            // Проверка логина и пароля в базе данных
            // Генерация JWT токена
            
            // Заглушка для демонстрации структуры
            var response = new Core.DTOs.Auth.AuthResponseDto
            {
                AccessToken = "fake-jwt-token",
                RefreshToken = "fake-refresh-token",
                ExpiresIn = DateTime.UtcNow.AddHours(1),
                User = new Core.DTOs.Auth.UserDto
                {
                    Id = Guid.NewGuid(),
                    Username = loginRequest.Username,
                    Email = "user@example.com",
                    FirstName = "Иван",
                    LastName = "Иванов",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Roles = new List<Core.DTOs.Auth.RoleDto>(),
                    Locations = new List<Core.DTOs.Auth.LocationDto>()
                }
            };
            
            return Ok(response);
        }
    }
}