using Microsoft.AspNetCore.Mvc;
using MakiMora.Core.DTOs.Auth;
using MakiMora.Core.Services;
using Microsoft.AspNetCore.Authorization;

namespace MakiMora.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "hr")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _userService.GetUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] Core.DTOs.Auth.CreateUserRequestDto createUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _userService.CreateUserAsync(createUserDto);
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserDto>> UpdateUser(Guid id, [FromBody] Core.DTOs.Auth.UpdateUserRequestDto updateUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _userService.UpdateUserAsync(id, updateUserDto);
                return Ok(user);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost("{userId}/roles/{roleId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AssignRoleToUser(Guid userId, Guid roleId)
        {
            var result = await _userService.AssignRoleToUserAsync(userId, roleId);
            if (!result)
            {
                return NotFound();
            }

            return Ok(new { message = "Role assigned successfully" });
        }

        [HttpDelete("{userId}/roles/{roleId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RemoveRoleFromUser(Guid userId, Guid roleId)
        {
            var result = await _userService.RemoveRoleFromUserAsync(userId, roleId);
            if (!result)
            {
                return NotFound();
            }

            return Ok(new { message = "Role removed successfully" });
        }

        [HttpPost("{userId}/locations/{locationId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AssignLocationToUser(Guid userId, Guid locationId)
        {
            var result = await _userService.AssignLocationToUserAsync(userId, locationId);
            if (!result)
            {
                return NotFound();
            }

            return Ok(new { message = "Location assigned successfully" });
        }

        [HttpDelete("{userId}/locations/{locationId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RemoveLocationFromUser(Guid userId, Guid locationId)
        {
            var result = await _userService.RemoveLocationFromUserAsync(userId, locationId);
            if (!result)
            {
                return NotFound();
            }

            return Ok(new { message = "Location removed successfully" });
        }
    }
}