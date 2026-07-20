using Inventory.Domain.Aggregates.Categories;
using Inventory.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Persistence.Configurations
{
    public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasConversion(
                    x => x.Value,
                    x => CategoryName.Create(x))
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.HasIndex(x => x.Name)
                .IsUnique();
        }
    }
}