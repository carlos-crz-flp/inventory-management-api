using Inventory.Application.Abstractions.Persistence;
using Inventory.Domain.Aggregates.Products;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence.Repositories
{
    public sealed class ProductRepository : IProductRepository
    {
        private readonly InventoryDbContext _context;

        public ProductRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
        {
            await _context.Products.AddAsync(product, cancellationToken);
        }

        public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .Include(x => x.Movements)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .Include(x => x.Movements)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .FirstOrDefaultAsync(x => x.Sku.Value == sku, cancellationToken);
        }

        public void Remove(Product product)
        {
            _context.Products.Remove(product);
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
        }
    }
}