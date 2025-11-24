using System.ComponentModel.DataAnnotations;

namespace MakiMora.Core.DTOs
{
    public class CreateCategoryRequestDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public Guid LocationId { get; set; }

        public bool IsActive { get; set; } = true;
        public int SortOrder { get; set; } = 0;
    }
}