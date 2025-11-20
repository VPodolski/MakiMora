using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MakiMora.Core.Entities
{
    public class OrderStatusHistory : BaseEntity
    {
        [Key]
        public new Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        public Guid OrderId { get; set; }
        
        [Required]
        public Guid StatusId { get; set; }
        
        public Guid? ChangedById { get; set; }
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
        public string? Note { get; set; }
        
        // Navigation properties
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; } = null!;
        
        [ForeignKey("StatusId")]
        public virtual OrderStatus Status { get; set; } = null!;
        
        [ForeignKey("ChangedById")]
        public virtual User? ChangedBy { get; set; }
    }
}