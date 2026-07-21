using Inventory.Application.Abstractions.Persistence;
using Inventory.Domain.Aggregates.Categories;
using Inventory.Domain.ValueObjects;
using MediatR;

namespace Inventory.Application.Features.Categories.CreateCategory
{
    public sealed class CreateCategoryCommandHandler
        : IRequestHandler<CreateCategoryCommand, Guid>
    {
        private readonly ICategoryCommandRepository _commandRepository;

        public CreateCategoryCommandHandler(
            ICategoryCommandRepository commandRepository)
        {
            _commandRepository = commandRepository;
        }

        public async Task<Guid> Handle(
            CreateCategoryCommand request,
            CancellationToken cancellationToken)
        {
            var category = new Category(
                CategoryName.Create(request.Name));

            await _commandRepository.AddAsync(
                category,
                cancellationToken);

            return category.Id;
        }
    }
}