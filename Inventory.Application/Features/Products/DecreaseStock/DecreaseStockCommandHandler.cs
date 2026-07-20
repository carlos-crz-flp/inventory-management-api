using Inventory.Application.Abstractions.Persistence;
using MediatR;

namespace Inventory.Application.Features.Products.DecreaseStock
{
    public sealed class DecreaseStockCommandHandler
    : IRequestHandler<DecreaseStockCommand>
    {
        private readonly IProductRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DecreaseStockCommandHandler(
            IProductRepository repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
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

            _repository.Update(product);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}