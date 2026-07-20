using Inventory.Application.Abstractions.Persistence;
using MediatR;

namespace Inventory.Application.Features.Products.DecreaseStock
{
    public sealed class DecreaseStockCommandHandler
        : IRequestHandler<DecreaseStockCommand>
    {
        private readonly IProductRepository _repository;
        private readonly IProductCommandRepository _commandRepository;

        public DecreaseStockCommandHandler(
            IProductRepository repository,
            IProductCommandRepository commandRepository)
        {
            _repository = repository;
            _commandRepository = commandRepository;
        }

        public async Task Handle(
            DecreaseStockCommand request,
            CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(
                request.ProductId,
                cancellationToken);

            if (product is null)
                throw new KeyNotFoundException("Product not found.");

            product.DecreaseStock(request.Quantity);

            await _commandRepository.SaveStockMovementAsync(
                product,
                product.LastMovement!,
                cancellationToken);
        }
    }
}