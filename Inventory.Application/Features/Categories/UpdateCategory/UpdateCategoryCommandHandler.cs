using Inventory.Application.Abstractions.Persistence;
using Inventory.Domain.ValueObjects;
using MediatR;

namespace Inventory.Application.Features.Categories.UpdateCategory
{
    public sealed class UpdateCategoryCommandHandler
        : IRequestHandler<UpdateCategoryCommand>
    {
        private readonly ICategoryRepository _queryRepository;
        private readonly ICategoryCommandRepository _commandRepository;

        public UpdateCategoryCommandHandler(
            ICategoryRepository queryRepository,
            ICategoryCommandRepository commandRepository)
        {
            _queryRepository = queryRepository;
            _commandRepository = commandRepository;
        }

        public async Task Handle(
            UpdateCategoryCommand request,
            CancellationToken cancellationToken)
        {
            var category = await _queryRepository.GetByIdAsync(
                request.Id,
                cancellationToken);

            if (category is null)
                throw new KeyNotFoundException("Category not found.");

            category.Rename(
                CategoryName.Create(request.Name));

            await _commandRepository.UpdateAsync(
                category,
                cancellationToken);
        }
    }
}