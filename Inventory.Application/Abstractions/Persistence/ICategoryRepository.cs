using Inventory.Domain.Aggregates.Categories;

namespace Inventory.Application.Abstractions.Persistence
{
    public interface ICategoryRepository
    {
        Task<Category?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<Category>> GetAllAsync(
            CancellationToken cancellationToken = default);
    }
}