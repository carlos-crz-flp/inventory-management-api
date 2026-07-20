using Inventory.Domain.Aggregates.Categories;

namespace Inventory.Application.Abstractions.Persistence
{
    public interface ICategoryCommandRepository
    {
        Task AddAsync(
            Category category,
            CancellationToken cancellationToken);

        Task UpdateAsync(
            Category category,
            CancellationToken cancellationToken);

        Task DeleteAsync(
            Guid id,
            CancellationToken cancellationToken);
    }
}