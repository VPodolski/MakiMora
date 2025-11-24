using System.ComponentModel.DataAnnotations;

namespace MakiMora.Core.DTOs
{
    public class UpdateProductRequestDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0")]
        public decimal Price { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        public string? ImageUrl { get; set; }

        public int? PreparationTime { get; set; }

        [Required]
        public bool IsAvailable { get; set; }
    }
}