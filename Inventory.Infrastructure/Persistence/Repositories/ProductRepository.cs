using Inventory.Application.Abstractions.Persistence;
using Inventory.Domain.Aggregates.Products;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence.Repositories
{
    public sealed class ProductRepository : IProductRepository
    {
        private readonly InventoryDbContext _context;

        public ProductRepository(
            InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Product>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .Include(x => x.Movements)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<Product?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .Include(x => x.Movements)
                .FirstOrDefaultAsync(
                    x => x.Id == id,
                    cancellationToken);
        }

        public async Task<Product?> GetBySkuAsync(
            string sku,
            CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .FirstOrDefaultAsync(
                    x => x.Sku.Value == sku,
                    cancellationToken);
        }
    }
}