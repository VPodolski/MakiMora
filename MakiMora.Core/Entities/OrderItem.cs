using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MakiMora.Core.Entities
{
    public class OrderItem
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        public Guid OrderId { get; set; }
        
        [Required]
        public Guid ProductId { get; set; }
        
        [Required]
        public int Quantity { get; set; } = 1;
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal UnitPrice { get; set; }
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalPrice { get; set; }
        
        [Required]
        public Guid StatusId { get; set; }
        
        public Guid? PreparedById { get; set; } // Sushi chef
        public DateTime? PreparedAt { get; set; }
        
        public Guid? AssembledById { get; set; } // Packer
        public DateTime? AssembledAt { get; set; }
        
        // Navigation properties
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; } = null!;
        
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; } = null!;
        
        [ForeignKey("StatusId")]
        public virtual OrderItemStatus Status { get; set; } = null!;
        
        [ForeignKey("PreparedById")]
        public virtual User? PreparedBy { get; set; }
        
        [ForeignKey("AssembledById")]
        public virtual User? AssembledBy { get; set; }
        
        public virtual ICollection<OrderItemStatusHistory> StatusHistories { get; set; } = new List<OrderItemStatusHistory>();
    }
}