using System.ComponentModel.DataAnnotations;

namespace MakiMora.Core.DTOs.Auth
{
    public class CreateUserRequestDto
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Phone { get; set; }

        public List<Guid>? RoleIds { get; set; }
        public List<Guid>? LocationIds { get; set; }
        public bool IsActive { get; set; } = true;
    }
}