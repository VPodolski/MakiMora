using Microsoft.AspNetCore.Mvc;
using MakiMora.Core.DTOs.Auth;
using MakiMora.Core.Services;
using Microsoft.AspNetCore.Authorization;
using MakiMora.Core.Configurations;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MakiMora.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtConfiguration _jwtConfig;

        public AuthController(IUserService userService, JwtConfiguration jwtConfig)
        {
            _userService = userService;
            _jwtConfig = jwtConfig;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isValid = await _userService.ValidateCredentialsAsync(loginRequest.Username, loginRequest.Password);
            if (!isValid)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            var userDto = await _userService.GetUserByUsernameAsync(loginRequest.Username);
            if (userDto == null)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            var token = GenerateJwtToken(userDto);
            var refreshToken = GenerateRefreshToken();

            var response = new AuthResponseDto
            {
                AccessToken = token,
                RefreshToken = refreshToken,
                ExpiresIn = DateTime.UtcNow.AddMinutes(_jwtConfig.ExpiryMinutes),
                User = userDto
            };

            return Ok(response);
        }

        private string GenerateJwtToken(Core.DTOs.Auth.UserDto user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // JWT ID
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            // Add roles to claims
            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtConfig.ExpiryMinutes),
                Issuer = _jwtConfig.Issuer,
                Audience = _jwtConfig.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            // In a real implementation, you would generate a secure random refresh token
            // and store it in a database with expiration date
            return Guid.NewGuid().ToString();
        }
    }
}