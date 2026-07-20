using Inventory.Domain.ValueObjects;

namespace Inventory.Domain.Aggregates.Categories
{
    public sealed class Category
    {
        public Guid Id { get; private set; }

        public CategoryName Name { get; private set; }

        public bool IsActive { get; private set; }

        private Category()
        {
        }

        public Category(CategoryName name)
        {
            Id = Guid.NewGuid();
            Name = name;
            IsActive = true;
        }

        public void Rename(CategoryName name)
        {
            Name = name ?? throw new DomainException("Category name is required.");
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }
    }
}