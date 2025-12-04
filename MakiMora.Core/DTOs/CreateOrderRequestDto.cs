using System.ComponentModel.DataAnnotations;

namespace MakiMora.Core.DTOs
{
    public class CreateOrderRequestDto
    {
        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string CustomerPhone { get; set; } = string.Empty;

        [Required]
        public string CustomerAddress { get; set; } = string.Empty;

        [Required]
        public Guid LocationId { get; set; }

        public decimal DeliveryFee { get; set; } = 0;

        public string? Comment { get; set; }

        public DateTime? DeliveryTime { get; set; }

        [Required]
        public List<CreateOrderItemRequestDto> Items { get; set; } = new List<CreateOrderItemRequestDto>();
    }

    public class CreateOrderItemRequestDto
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;
    }
}