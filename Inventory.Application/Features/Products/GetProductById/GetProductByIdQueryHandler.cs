using Inventory.Application.Abstractions.Persistence;
using Inventory.Domain.Aggregates.Products;
using MediatR;

namespace Inventory.Application.Features.Products.GetProductById
{
    public sealed class GetProductByIdQueryHandler
    : IRequestHandler<GetProductByIdQuery, Product?>
    {
        private readonly IProductRepository _repository;

        public GetProductByIdQueryHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<Product?> Handle(
            GetProductByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(
                request.Id,
                cancellationToken);
        }
    }
}