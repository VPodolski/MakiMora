using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MakiMora.Core.Entities
{
    public class InventorySupplyItem : BaseEntity
    {
        [Key]
        public new Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        public Guid SupplyId { get; set; }
        
        public Guid? ProductId { get; set; } // Can be null if new product
        
        [Required]
        [MaxLength(100)]
        public string ProductName { get; set; } = string.Empty; // Name of product at supply time
        
        [Required]
        public int Quantity { get; set; }
        
        [Column(TypeName = "decimal(10,2)")]
        [Required]
        public decimal UnitCost { get; set; }
        
        [Column(TypeName = "decimal(10,2)")]
        [Required]
        public decimal TotalCost { get; set; }
        
        // Navigation properties
        [ForeignKey("SupplyId")]
        public virtual InventorySupply Supply { get; set; } = null!;
        
        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }
    }
}