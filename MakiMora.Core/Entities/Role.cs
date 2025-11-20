using System.ComponentModel.DataAnnotations;

namespace MakiMora.Core.Entities
{
    public class Role : BaseEntity
    {
        [Key]
        public new Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}