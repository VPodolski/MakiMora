using System.ComponentModel.DataAnnotations;

namespace MakiMora.Core.Entities
{
    public class Location : BaseEntity
    {
        [Key]
        public new Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        [MaxLength(10)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public string Address { get; set; } = string.Empty;
        
        [MaxLength(20)]
        public string? Phone { get; set; }
        
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual ICollection<UserLocation> UserLocations { get; set; } = new List<UserLocation>();
        public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<InventorySupply> Supplies { get; set; } = new List<InventorySupply>();
    }
}