using Inventory.Domain.Enums;
using Inventory.Domain.ValueObjects;

namespace Inventory.Domain.Aggregates.Products
{
    public sealed class Product
    {
        private readonly List<InventoryMovement> _movements = [];

        public Guid Id { get; private set; }

        public Sku Sku { get; private set; }

        public ProductName Name { get; private set; }

        public Guid CategoryId { get; private set; }

        public Quantity Stock { get; private set; }

        public bool IsActive { get; private set; }

        public IReadOnlyCollection<InventoryMovement> Movements => _movements.AsReadOnly();

        private Product()
        {
        }

        public Product(
            Sku sku,
            ProductName name,
            Guid categoryId)
        {
            Id = Guid.NewGuid();
            Sku = sku;
            Name = name;
            CategoryId = categoryId;
            Stock = Quantity.Create(0);
            IsActive = true;
        }

        public void Rename(ProductName name)
        {
            Name = name ?? throw new DomainException("Product name is required.");
        }

        public void ChangeCategory(Guid categoryId)
        {
            CategoryId = categoryId;
        }

        public void IncreaseStock(int quantity)
        {
            Stock = Stock.Add(quantity);

            _movements.Add(
                new InventoryMovement(
                    Id,
                    InventoryMovementType.Entry,
                    quantity));
        }

        public void DecreaseStock(int quantity)
        {
            Stock = Stock.Subtract(quantity);

            _movements.Add(
                new InventoryMovement(
                    Id,
                    InventoryMovementType.Exit,
                    quantity));
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