using Inventory.Domain.Aggregates.Categories;

namespace Inventory.Application.Abstractions.Persistence
{
    public interface ICategoryRepository
    {
        Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken cancellationToken = default);

        Task AddAsync(Category category, CancellationToken cancellationToken = default);

        void Update(Category category);

        void Remove(Category category);
    }
}