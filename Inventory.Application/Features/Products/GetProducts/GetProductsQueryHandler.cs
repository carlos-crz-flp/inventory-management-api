using Inventory.Application.Abstractions.Persistence;
using Inventory.Domain.Aggregates.Products;
using MediatR;

namespace Inventory.Application.Features.Products.GetProducts
{
    public sealed class GetProductsQueryHandler
    : IRequestHandler<GetProductsQuery, IReadOnlyList<Product>>
    {
        private readonly IProductRepository _repository;

        public GetProductsQueryHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<Product>> Handle(
            GetProductsQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync(cancellationToken);
        }
    }
}