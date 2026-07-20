using Inventory.Application.Abstractions.Persistence;
using Inventory.Domain.Aggregates.Products;
using Inventory.Domain.ValueObjects;
using MediatR;

namespace Inventory.Application.Features.Products.CreateProduct
{
    public sealed class CreateProductCommandHandler
    : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductCommandHandler(
            IProductRepository repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(
            CreateProductCommand request,
            CancellationToken cancellationToken)
        {
            var product = new Product(
                Sku.Create(request.Sku),
                ProductName.Create(request.Name),
                request.CategoryId);

            await _repository.AddAsync(product, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return product.Id;
        }
    }
}