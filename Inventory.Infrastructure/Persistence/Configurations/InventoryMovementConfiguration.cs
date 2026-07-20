using Inventory.Domain.Aggregates.Products;
using Inventory.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Persistence.Configurations
{
    public sealed class InventoryMovementConfiguration
    : IEntityTypeConfiguration<InventoryMovement>
    {
        public void Configure(EntityTypeBuilder<InventoryMovement> builder)
        {
            builder.ToTable("InventoryMovements");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Type)
                .HasConversion<int>();

            builder.Property(x => x.Quantity)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();
        }
    }
}