using System.ComponentModel.DataAnnotations;

namespace MakiMora.Core.DTOs
{
    public class UpdateLocationRequestDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Phone { get; set; }

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }
}