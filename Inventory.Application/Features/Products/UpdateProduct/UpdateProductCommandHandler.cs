using Inventory.Application.Abstractions.Persistence;
using Inventory.Domain.ValueObjects;
using MediatR;

namespace Inventory.Application.Features.Products.UpdateProduct
{
    public sealed class UpdateProductCommandHandler
        : IRequestHandler<UpdateProductCommand>
    {
        private readonly IProductRepository _repository;
        private readonly IProductCommandRepository _commandRepository;

        public UpdateProductCommandHandler(
            IProductRepository repository,
            IProductCommandRepository commandRepository)
        {
            _repository = repository;
            _commandRepository = commandRepository;
        }

        public async Task Handle(
            UpdateProductCommand request,
            CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(
                request.Id,
                cancellationToken);

            if (product is null)
                throw new KeyNotFoundException("Product not found.");

            product.Rename(ProductName.Create(request.Name));
            product.ChangeCategory(request.CategoryId);

            await _commandRepository.UpdateAsync(
                product,
                cancellationToken);
        }
    }
}