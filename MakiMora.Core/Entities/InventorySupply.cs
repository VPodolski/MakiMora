using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MakiMora.Core.Entities
{
    public class InventorySupply : BaseEntity
    {
        [Key]
        public new Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        public Guid LocationId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string SupplierName { get; set; } = string.Empty;
        
        [Required]
        public DateTime SupplyDate { get; set; }
        
        [Required]
        public DateTime ExpectedDate { get; set; }
        
        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "pending"; // pending, delivered, cancelled
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal? TotalCost { get; set; }
        
        [Required]
        public Guid ManagerId { get; set; } // Manager of the location
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeliveredAt { get; set; }
        
        // Navigation properties
        [ForeignKey("LocationId")]
        public virtual Location Location { get; set; } = null!;
        
        [ForeignKey("ManagerId")]
        public virtual User Manager { get; set; } = null!;
        
        public virtual ICollection<InventorySupplyItem> Items { get; set; } = new List<InventorySupplyItem>();
    }
}