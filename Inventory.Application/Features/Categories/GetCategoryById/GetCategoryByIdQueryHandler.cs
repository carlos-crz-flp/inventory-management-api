using Inventory.Application.Abstractions.Persistence;
using Inventory.Domain.Aggregates.Categories;
using MediatR;

namespace Inventory.Application.Features.Categories.GetCategoryById
{
    public sealed class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, Category?>
    {
        private readonly ICategoryRepository _repository;

        public GetCategoryByIdQueryHandler(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<Category?> Handle(
            GetCategoryByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.Id, cancellationToken);
        }
    }
}