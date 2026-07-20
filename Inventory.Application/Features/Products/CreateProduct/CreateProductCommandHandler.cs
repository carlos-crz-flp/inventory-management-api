using Inventory.Application.Abstractions.Persistence;
using Inventory.Domain.Aggregates.Products;
using Inventory.Domain.ValueObjects;
using MediatR;

namespace Inventory.Application.Features.Products.CreateProduct
{
    public sealed class CreateProductCommandHandler
        : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductCommandRepository _commandRepository;

        public CreateProductCommandHandler(
            IProductCommandRepository commandRepository)
        {
            _commandRepository = commandRepository;
        }

        public async Task<Guid> Handle(
            CreateProductCommand request,
            CancellationToken cancellationToken)
        {
            var product = new Product(
                Sku.Create(request.Sku),
                ProductName.Create(request.Name),
                request.CategoryId);

            await _commandRepository.AddAsync(product, cancellationToken);

            return product.Id;
        }
    }
}