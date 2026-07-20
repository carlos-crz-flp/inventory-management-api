using Inventory.Domain.Aggregates.Products;

namespace Inventory.Application.Abstractions.Persistence
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default);

        Task AddAsync(Product product, CancellationToken cancellationToken = default);

        void Update(Product product);

        void Remove(Product product);
    }
}