using System.ComponentModel.DataAnnotations;

namespace MakiMora.Core.DTOs
{
    public class UpdateOrderItemStatusRequestDto
    {
        [Required]
        public Guid StatusId { get; set; }
        
        public string? Note { get; set; }
    }
}