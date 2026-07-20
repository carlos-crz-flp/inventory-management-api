using Inventory.Domain.Aggregates.Categories;
using Inventory.Domain.Aggregates.Products;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence
{
    public sealed class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories => Set<Category>();

        public DbSet<Product> Products => Set<Product>();

        public DbSet<InventoryMovement> InventoryMovements => Set<InventoryMovement>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(InventoryDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}