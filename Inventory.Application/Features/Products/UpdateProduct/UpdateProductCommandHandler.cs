using Inventory.Application.Abstractions.Persistence;
using Inventory.Domain.ValueObjects;
using MediatR;

namespace Inventory.Application.Features.Products.UpdateProduct
{
    public sealed class UpdateProductCommandHandler
    : IRequestHandler<UpdateProductCommand>
    {
        private readonly IProductRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCommandHandler(
            IProductRepository repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
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

            _repository.Update(product);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}