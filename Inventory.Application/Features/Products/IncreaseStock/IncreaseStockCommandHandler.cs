using Inventory.Application.Abstractions.Persistence;
using MediatR;

namespace Inventory.Application.Features.Products.IncreaseStock
{
    public sealed class IncreaseStockCommandHandler
    : IRequestHandler<IncreaseStockCommand>
    {
        private readonly IProductRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public IncreaseStockCommandHandler(
            IProductRepository repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
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

            _repository.Update(product);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}