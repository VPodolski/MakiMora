using System.ComponentModel.DataAnnotations;

namespace MakiMora.Core.DTOs
{
    public class UpdateProductAvailabilityRequestDto
    {
        [Required]
        public bool IsAvailable { get; set; }
    }
    
    public class UpdateProductStopListStatusRequestDto
    {
        [Required]
        public bool IsOnStopList { get; set; }
    }
}