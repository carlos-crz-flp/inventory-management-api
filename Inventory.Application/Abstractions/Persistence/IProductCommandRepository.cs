using Inventory.Domain.Aggregates.Products;

namespace Inventory.Application.Abstractions.Persistence
{
    public interface IProductCommandRepository
    {
        Task AddAsync(
            Product product,
            CancellationToken cancellationToken);

        Task UpdateAsync(
            Product product,
            CancellationToken cancellationToken);

        Task DeleteAsync(
            Guid id,
            CancellationToken cancellationToken);

        Task SaveStockMovementAsync(
            Product product,
            InventoryMovement movement,
            CancellationToken cancellationToken);
    }
}