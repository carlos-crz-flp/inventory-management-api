using Inventory.Application.Abstractions.Persistence;
using MediatR;

namespace Inventory.Application.Features.Categories.DeleteCategory
{
    public sealed class DeleteCategoryCommandHandler
        : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly ICategoryRepository _queryRepository;
        private readonly ICategoryCommandRepository _commandRepository;

        public DeleteCategoryCommandHandler(
            ICategoryRepository queryRepository,
            ICategoryCommandRepository commandRepository)
        {
            _queryRepository = queryRepository;
            _commandRepository = commandRepository;
        }

        public async Task Handle(
            DeleteCategoryCommand request,
            CancellationToken cancellationToken)
        {
            var category = await _queryRepository.GetByIdAsync(
                request.Id,
                cancellationToken);

            if (category is null)
                throw new KeyNotFoundException("Category not found.");

            category.Deactivate();

            await _commandRepository.UpdateAsync(
                category,
                cancellationToken);
        }
    }
}