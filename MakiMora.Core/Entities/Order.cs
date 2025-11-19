using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MakiMora.Core.Entities
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        [MaxLength(20)]
        public string OrderNumber { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string CustomerName { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(20)]
        public string CustomerPhone { get; set; } = string.Empty;
        
        [Required]
        public string CustomerAddress { get; set; } = string.Empty;
        
        [Required]
        public Guid LocationId { get; set; }
        
        [Required]
        public Guid StatusId { get; set; }
        
        public Guid? CourierId { get; set; } // Will be assigned later
        public Guid? AssemblerId { get; set; } // Packer
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal DeliveryFee { get; set; } = 0;
        
        public string? Comment { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeliveryTime { get; set; }
        public DateTime? CompletedAt { get; set; }
        
        // Navigation properties
        [ForeignKey("LocationId")]
        public virtual Location Location { get; set; } = null!;
        
        [ForeignKey("StatusId")]
        public virtual OrderStatus Status { get; set; } = null!;
        
        [ForeignKey("CourierId")]
        public virtual User? Courier { get; set; }
        
        [ForeignKey("AssemblerId")]
        public virtual User? Assembler { get; set; }
        
        public virtual ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
        public virtual ICollection<OrderStatusHistory> StatusHistories { get; set; } = new List<OrderStatusHistory>();
    }
}