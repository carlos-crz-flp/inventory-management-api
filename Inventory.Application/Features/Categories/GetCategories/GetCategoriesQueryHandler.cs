using Inventory.Application.Abstractions.Persistence;
using Inventory.Domain.Aggregates.Categories;
using MediatR;

namespace Inventory.Application.Features.Categories.GetCategories
{
    public sealed class GetCategoriesQueryHandler
    : IRequestHandler<GetCategoriesQuery, IReadOnlyList<Category>>
    {
        private readonly ICategoryRepository _repository;

        public GetCategoriesQueryHandler(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<Category>> Handle(
            GetCategoriesQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync(cancellationToken);
        }
    }
}