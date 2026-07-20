using Inventory.Domain.Aggregates.Products;
using Inventory.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Persistence.Configurations
{
    public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Sku)
                .HasConversion(
                    x => x.Value,
                    x => Sku.Create(x))
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Name)
                .HasConversion(
                    x => x.Value,
                    x => ProductName.Create(x))
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Stock)
                .HasConversion(
                    x => x.Value,
                    x => Quantity.Create(x))
                .IsRequired();

            builder.Property(x => x.CategoryId)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.HasIndex(x => x.Sku)
                .IsUnique();

            builder.HasMany(x => x.Movements)
                .WithOne()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(x => x.Movements)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}