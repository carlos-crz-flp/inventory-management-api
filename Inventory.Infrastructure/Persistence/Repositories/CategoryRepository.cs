using Inventory.Application.Abstractions.Persistence;
using Inventory.Domain.Aggregates.Categories;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence.Repositories
{
    public sealed class CategoryRepository : ICategoryRepository
    {
        private readonly InventoryDbContext _context;

        public CategoryRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Category category, CancellationToken cancellationToken = default)
        {
            await _context.Categories.AddAsync(category, cancellationToken);
        }

        public async Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Categories
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public void Remove(Category category)
        {
            _context.Categories.Remove(category);
        }

        public void Update(Category category)
        {
            _context.Categories.Update(category);
        }
    }
}