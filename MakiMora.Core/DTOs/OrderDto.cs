using System.ComponentModel.DataAnnotations;

namespace MakiMora.Core.DTOs
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string CustomerAddress { get; set; } = string.Empty;
        public Guid LocationId { get; set; }
        public OrderStatusDto Status { get; set; } = null!;
        public UserDto? Courier { get; set; }
        public UserDto? Assembler { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DeliveryFee { get; set; }
        public string? Comment { get; set; }
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeliveryTime { get; set; }
        public DateTime? CompletedAt { get; set; }
    }

    public class OrderItemDto
    {
        public Guid Id { get; set; }
        public ProductDto Product { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderItemStatusDto Status { get; set; } = null!;
        public UserDto? PreparedBy { get; set; }
        public DateTime? PreparedAt { get; set; }
        public UserDto? AssembledBy { get; set; }
        public DateTime? AssembledAt { get; set; }
    }

    public class OrderStatusDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
    }

    public class OrderItemStatusDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
    }
}