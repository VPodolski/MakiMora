using System.ComponentModel.DataAnnotations;
using MakiMora.Core.Entities;

namespace MakiMora.Core.DTOs
{
    public class InventorySupplyDto
    {
        public Guid Id { get; set; }
        public Guid LocationId { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public DateTime SupplyDate { get; set; }
        public DateTime ExpectedDate { get; set; }
        public string Status { get; set; } = string.Empty; // pending, delivered, cancelled
        public decimal? TotalCost { get; set; }
        public Guid ManagerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public List<InventorySupplyItemDto> Items { get; set; } = new List<InventorySupplyItemDto>();
    }

    public class InventorySupplyItemDto
    {
        public Guid Id { get; set; }
        public Guid SupplyId { get; set; }
        public Guid? ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalCost { get; set; }
    }

    public class CreateInventorySupplyRequestDto
    {
        [Required]
        public Guid LocationId { get; set; }

        [Required]
        [StringLength(100)]
        public string SupplierName { get; set; } = string.Empty;

        [Required]
        public DateTime SupplyDate { get; set; }

        [Required]
        public DateTime ExpectedDate { get; set; }

        [Required]
        public List<CreateInventorySupplyItemRequestDto> Items { get; set; } = new List<CreateInventorySupplyItemRequestDto>();
        
        [Required]
        public Guid ManagerId { get; set; }
    }

    public class CreateInventorySupplyItemRequestDto
    {
        public Guid? ProductId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal UnitCost { get; set; }
    }

    public class UpdateInventorySupplyRequestDto
    {
        [Required]
        [StringLength(100)]
        public string SupplierName { get; set; } = string.Empty;

        [Required]
        public DateTime SupplyDate { get; set; }

        [Required]
        public DateTime ExpectedDate { get; set; }

        public string Status { get; set; } = "pending"; // pending, delivered, cancelled
        
        [Required]
        public Guid ManagerId { get; set; }
    }

    public class UpdateSupplyStatusRequestDto
    {
        [Required]
        public string Status { get; set; } = string.Empty; // pending, delivered, cancelled
    }
}