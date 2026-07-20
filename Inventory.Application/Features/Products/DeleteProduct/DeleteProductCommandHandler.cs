using Inventory.Application.Abstractions.Persistence;
using MediatR;

namespace Inventory.Application.Features.Products.DeleteProduct
{
    public sealed class DeleteProductCommandHandler
    : IRequestHandler<DeleteProductCommand>
    {
        private readonly IProductRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductCommandHandler(
            IProductRepository repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(
            DeleteProductCommand request,
            CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(
                request.Id,
                cancellationToken);

            if (product is null)
                throw new KeyNotFoundException("Product not found.");

            product.Deactivate();

            _repository.Update(product);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}