using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MakiMora.Core.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;
        
        [MaxLength(20)]
        public string? Phone { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        
        // Navigation properties
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public virtual ICollection<UserLocation> UserLocations { get; set; } = new List<UserLocation>();
        public virtual ICollection<Order> AssignedOrders { get; set; } = new List<Order>(); // Courier assigned orders
        public virtual ICollection<Order> AssembledOrders { get; set; } = new List<Order>(); // Orders assembled by this user
        public virtual ICollection<OrderItem> PreparedOrderItems { get; set; } = new List<OrderItem>(); // Items prepared by this user
        public virtual ICollection<OrderItem> AssembledOrderItems { get; set; } = new List<OrderItem>(); // Items assembled by this user
        public virtual ICollection<InventorySupply> Supplies { get; set; } = new List<InventorySupply>(); // Supplies managed by this user
        public virtual ICollection<CourierEarning> Earnings { get; set; } = new List<CourierEarning>();
    }
}