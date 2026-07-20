using Inventory.Domain.Enums;

namespace Inventory.Domain.Aggregates.Products
{
    public sealed class InventoryMovement
    {
        public Guid Id { get; private set; }

        public Guid ProductId { get; private set; }

        public InventoryMovementType Type { get; private set; }

        public int Quantity { get; private set; }

        public DateTime CreatedAt { get; private set; }

        private InventoryMovement()
        {
        }

        public InventoryMovement(
            Guid productId,
            InventoryMovementType type,
            int quantity)
        {
            Id = Guid.NewGuid();
            ProductId = productId;
            Type = type;
            Quantity = quantity;
            CreatedAt = DateTime.UtcNow;
        }
    }
}