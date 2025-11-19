using Microsoft.EntityFrameworkCore;
using MakiMora.Core.Entities;

namespace MakiMora.Infrastructure.Data
{
    public class MakiMoraDbContext : DbContext
    {
        public MakiMoraDbContext(DbContextOptions<MakiMoraDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<UserRole> UserRoles { get; set; } = null!;
        public DbSet<Location> Locations { get; set; } = null!;
        public DbSet<UserLocation> UserLocations { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;
        public DbSet<OrderStatus> OrderStatuses { get; set; } = null!;
        public DbSet<OrderItemStatus> OrderItemStatuses { get; set; } = null!;
        public DbSet<InventorySupply> InventorySupplies { get; set; } = null!;
        public DbSet<InventorySupplyItem> InventorySupplyItems { get; set; } = null!;
        public DbSet<OrderStatusHistory> OrderStatusHistories { get; set; } = null!;
        public DbSet<OrderItemStatusHistory> OrderItemStatusHistories { get; set; } = null!;
        public DbSet<CourierEarning> CourierEarnings { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Username).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(100).IsRequired();
                entity.Property(e => e.FirstName).HasMaxLength(50).IsRequired();
                entity.Property(e => e.LastName).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
                
                // Configure relationships
                entity.HasMany(u => u.UserRoles)
                    .WithOne(ur => ur.User)
                    .HasForeignKey(ur => ur.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasMany(u => u.UserLocations)
                    .WithOne(ul => ul.User)
                    .HasForeignKey(ul => ul.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasMany(u => u.AssignedOrders)
                    .WithOne(o => o.Courier)
                    .HasForeignKey(o => o.CourierId)
                    .OnDelete(DeleteBehavior.SetNull);
                    
                entity.HasMany(u => u.AssembledOrders)
                    .WithOne(o => o.Assembler)
                    .HasForeignKey(o => o.AssemblerId)
                    .OnDelete(DeleteBehavior.SetNull);
                    
                entity.HasMany(u => u.PreparedOrderItems)
                    .WithOne(oi => oi.PreparedBy)
                    .HasForeignKey(oi => oi.PreparedById)
                    .OnDelete(DeleteBehavior.SetNull);
                    
                entity.HasMany(u => u.AssembledOrderItems)
                    .WithOne(oi => oi.AssembledBy)
                    .HasForeignKey(oi => oi.AssembledById)
                    .OnDelete(DeleteBehavior.SetNull);
                    
                entity.HasMany(u => u.Supplies)
                    .WithOne(supply => supply.Manager)
                    .HasForeignKey(supply => supply.ManagerId)
                    .OnDelete(DeleteBehavior.Restrict);
                    
                entity.HasMany(u => u.Earnings)
                    .WithOne(ce => ce.Courier)
                    .HasForeignKey(ce => ce.CourierId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Role entity
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
                entity.HasIndex(e => e.Name).IsUnique();
                
                // Configure relationships
                entity.HasMany(r => r.UserRoles)
                    .WithOne(ur => ur.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure UserRole entity
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                
                // Composite unique index
                entity.HasIndex(ur => new { ur.UserId, ur.RoleId }).IsUnique();
            });

            // Configure Location entity
            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired(); // Исправил с 10 на 100
                entity.Property(e => e.Phone).HasMaxLength(20);
                
                // Configure relationships
                entity.HasMany(l => l.UserLocations)
                    .WithOne(ul => ul.Location)
                    .HasForeignKey(ul => ul.LocationId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasMany(l => l.Categories)
                    .WithOne(c => c.Location)
                    .HasForeignKey(c => c.LocationId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasMany(l => l.Products)
                    .WithOne(p => p.Location)
                    .HasForeignKey(p => p.LocationId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasMany(l => l.Orders)
                    .WithOne(o => o.Location)
                    .HasForeignKey(o => o.LocationId)
                    .OnDelete(DeleteBehavior.Restrict);
                    
                entity.HasMany(l => l.Supplies)
                    .WithOne(supply => supply.Location)
                    .HasForeignKey(supply => supply.LocationId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure UserLocation entity
            modelBuilder.Entity<UserLocation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                
                // Composite unique index
                entity.HasIndex(ul => new { ul.UserId, ul.LocationId }).IsUnique();
            });

            // Configure Category entity
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired(); // Исправил с 10 на 100
                entity.Property(e => e.Description);
                
                // Configure relationships
                entity.HasMany(c => c.Products)
                    .WithOne(p => p.Category)
                    .HasForeignKey(p => p.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Product entity
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired(); // Исправил с 10 на 100
                entity.Property(e => e.Price).HasColumnType("decimal(10,2)");
                entity.Property(e => e.ImageUrl);
                entity.Property(e => e.Description);
                
                // Configure relationships
                entity.HasMany(p => p.OrderItems)
                    .WithOne(oi => oi.Product)
                    .HasForeignKey(oi => oi.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure OrderStatus entity
            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
                entity.Property(e => e.DisplayName).HasMaxLength(100).IsRequired();
                entity.HasIndex(e => e.Name).IsUnique();
                
                // Configure relationships
                entity.HasMany(os => os.Orders)
                    .WithOne(o => o.Status)
                    .HasForeignKey(o => o.StatusId)
                    .OnDelete(DeleteBehavior.Restrict);
                    
                entity.HasMany(os => os.StatusHistories)
                    .WithOne(osh => osh.Status)
                    .HasForeignKey(osh => osh.StatusId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure OrderItemStatus entity
            modelBuilder.Entity<OrderItemStatus>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
                entity.Property(e => e.DisplayName).HasMaxLength(100).IsRequired();
                entity.HasIndex(e => e.Name).IsUnique();
                
                // Configure relationships
                entity.HasMany(ois => ois.OrderItems)
                    .WithOne(oi => oi.Status)
                    .HasForeignKey(oi => oi.StatusId)
                    .OnDelete(DeleteBehavior.Restrict);
                    
                entity.HasMany(ois => ois.StatusHistories)
                    .WithOne(oish => oish.Status)
                    .HasForeignKey(oish => oish.StatusId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Order entity
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.OrderNumber).HasMaxLength(20).IsRequired();
                entity.Property(e => e.CustomerName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.CustomerPhone).HasMaxLength(20).IsRequired();
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(10,2)");
                entity.Property(e => e.DeliveryFee).HasColumnType("decimal(10,2)");
                entity.HasIndex(e => e.OrderNumber).IsUnique();
                
                // Configure relationships
                entity.HasMany(o => o.Items)
                    .WithOne(oi => oi.Order)
                    .HasForeignKey(oi => oi.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasMany(o => o.StatusHistories)
                    .WithOne(osh => osh.Order)
                    .HasForeignKey(osh => osh.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure OrderItem entity
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(10,2)").IsRequired();
                entity.Property(e => e.TotalPrice).HasColumnType("decimal(10,2)").IsRequired();
                
                // Configure relationships
                entity.HasMany(oi => oi.StatusHistories)
                    .WithOne(oish => oish.OrderItem)
                    .HasForeignKey(oish => oish.OrderItemId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure InventorySupply entity
            modelBuilder.Entity<InventorySupply>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.SupplierName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Status).HasMaxLength(20).IsRequired();
                entity.Property(e => e.TotalCost).HasColumnType("decimal(10,2)");
                
                // Configure relationships
                entity.HasMany(supply => supply.Items)
                    .WithOne(item => item.Supply)
                    .HasForeignKey(item => item.SupplyId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure InventorySupplyItem entity
            modelBuilder.Entity<InventorySupplyItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.ProductName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.UnitCost).HasColumnType("decimal(10,2)").IsRequired();
                entity.Property(e => e.TotalCost).HasColumnType("decimal(10,2)").IsRequired();
            });

            // Configure OrderStatusHistory entity
            modelBuilder.Entity<OrderStatusHistory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            // Configure OrderItemStatusHistory entity
            modelBuilder.Entity<OrderItemStatusHistory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            // Configure CourierEarning entity
            modelBuilder.Entity<CourierEarning>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Amount).HasColumnType("decimal(10,2)").IsRequired();
                entity.Property(e => e.EarningType).HasMaxLength(20).IsRequired();
                
                // Configure relationships
                entity.HasOne(ce => ce.Order)
                    .WithMany() // Исправил, так как у Order нет навигационного свойства для CourierEarnings
                    .HasForeignKey(ce => ce.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Seed initial data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Order Statuses
            modelBuilder.Entity<OrderStatus>().HasData(
                new OrderStatus { Id = Guid.NewGuid(), Name = "pending", DisplayName = "В обработке", Description = "Заказ принят и ожидает обработки", SortOrder = 1, IsActive = true },
                new OrderStatus { Id = Guid.NewGuid(), Name = "confirmed", DisplayName = "Подтвержден", Description = "Заказ подтвержден менеджером точки", SortOrder = 2, IsActive = true },
                new OrderStatus { Id = Guid.NewGuid(), Name = "preparing", DisplayName = "Готовится", Description = "Заказ находится в процессе приготовления", SortOrder = 3, IsActive = true },
                new OrderStatus { Id = Guid.NewGuid(), Name = "ready", DisplayName = "Готов к сборке", Description = "Блюда приготовлены, ожидает упаковки", SortOrder = 4, IsActive = true },
                new OrderStatus { Id = Guid.NewGuid(), Name = "assembled", DisplayName = "Собран", Description = "Заказ собран, ожидает курьера", SortOrder = 5, IsActive = true },
                new OrderStatus { Id = Guid.NewGuid(), Name = "picked_up", DisplayName = "Забран курьером", Description = "Курьер забрал заказ", SortOrder = 6, IsActive = true },
                new OrderStatus { Id = Guid.NewGuid(), Name = "delivered", DisplayName = "Доставлен", Description = "Заказ успешно доставлен", SortOrder = 7, IsActive = true },
                new OrderStatus { Id = Guid.NewGuid(), Name = "cancelled", DisplayName = "Отменен", Description = "Заказ отменен", SortOrder = 8, IsActive = true }
            );

            // Seed Order Item Statuses
            modelBuilder.Entity<OrderItemStatus>().HasData(
                new OrderItemStatus { Id = Guid.NewGuid(), Name = "pending", DisplayName = "В ожидании", Description = "Позиция ожидает приготовления", SortOrder = 1, IsActive = true },
                new OrderItemStatus { Id = Guid.NewGuid(), Name = "preparing", DisplayName = "Готовится", Description = "Позиция в процессе приготовления", SortOrder = 2, IsActive = true },
                new OrderItemStatus { Id = Guid.NewGuid(), Name = "prepared", DisplayName = "Приготовлена", Description = "Позиция приготовлена, ожидает сборки", SortOrder = 3, IsActive = true },
                new OrderItemStatus { Id = Guid.NewGuid(), Name = "assembled", DisplayName = "Упакована", Description = "Позиция упакована в заказ", SortOrder = 4, IsActive = true },
                new OrderItemStatus { Id = Guid.NewGuid(), Name = "delivered", DisplayName = "Доставлена", Description = "Позиция доставлена", SortOrder = 5, IsActive = true },
                new OrderItemStatus { Id = Guid.NewGuid(), Name = "cancelled", DisplayName = "Отменена", Description = "Позиция отменена", SortOrder = 6, IsActive = true }
            );

            // Seed Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = Guid.NewGuid(), Name = "hr", Description = "Сотрудник отдела кадров" },
                new Role { Id = Guid.NewGuid(), Name = "manager", Description = "Менеджер точки" },
                new Role { Id = Guid.NewGuid(), Name = "sushi_chef", Description = "Сушист" },
                new Role { Id = Guid.NewGuid(), Name = "packer", Description = "Упаковщик" },
                new Role { Id = Guid.NewGuid(), Name = "courier", Description = "Курьер" }
            );
        }
    }
}