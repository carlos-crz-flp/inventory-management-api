using Inventory.Application.Abstractions.Persistence;
using MediatR;

namespace Inventory.Application.Features.Products.IncreaseStock
{
    public sealed class IncreaseStockCommandHandler
        : IRequestHandler<IncreaseStockCommand>
    {
        private readonly IProductRepository _repository;
        private readonly IProductCommandRepository _commandRepository;

        public IncreaseStockCommandHandler(
            IProductRepository repository,
            IProductCommandRepository commandRepository)
        {
            _repository = repository;
            _commandRepository = commandRepository;
        }

        public async Task Handle(
            IncreaseStockCommand request,
            CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(
                request.ProductId,
                cancellationToken);

            if (product is null)
                throw new KeyNotFoundException("Product not found.");

            product.IncreaseStock(request.Quantity);

            await _commandRepository.SaveStockMovementAsync(
                product,
                product.LastMovement!,
                cancellationToken);
        }
    }
}