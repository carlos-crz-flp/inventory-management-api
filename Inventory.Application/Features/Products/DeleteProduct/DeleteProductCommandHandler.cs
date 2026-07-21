using Inventory.Application.Abstractions.Persistence;
using MediatR;

namespace Inventory.Application.Features.Products.DeleteProduct
{
    public sealed class DeleteProductCommandHandler
        : IRequestHandler<DeleteProductCommand>
    {
        private readonly IProductRepository _queryRepository;
        private readonly IProductCommandRepository _commandRepository;

        public DeleteProductCommandHandler(
            IProductRepository queryRepository,
            IProductCommandRepository commandRepository)
        {
            _queryRepository = queryRepository;
            _commandRepository = commandRepository;
        }

        public async Task Handle(
            DeleteProductCommand request,
            CancellationToken cancellationToken)
        {
            var product = await _queryRepository.GetByIdAsync(
                request.Id,
                cancellationToken);

            if (product is null)
                throw new KeyNotFoundException("Product not found.");

            product.Deactivate();

            await _commandRepository.UpdateAsync(
                product,
                cancellationToken);
        }
    }
}