using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MakiMora.Core.Entities
{
    public class CourierEarning
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        public Guid CourierId { get; set; }
        
        [Required]
        public Guid OrderId { get; set; }
        
        [Column(TypeName = "decimal(10,2)")]
        [Required]
        public decimal Amount { get; set; }
        
        [Required]
        [MaxLength(20)]
        public string EarningType { get; set; } = string.Empty; // delivery_fee, bonus, penalty
        
        [Required]
        public DateTime Date { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        [ForeignKey("CourierId")]
        public virtual User Courier { get; set; } = null!;
        
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; } = null!;
    }
}