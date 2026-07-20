using Inventory.Application.Abstractions.Persistence;
using Inventory.Domain.Aggregates.Categories;
using Inventory.Domain.ValueObjects;
using MediatR;

namespace Inventory.Application.Features.Categories.CreateCategory
{
    public sealed class CreateCategoryCommandHandler
    : IRequestHandler<CreateCategoryCommand, Guid>
    {
        private readonly ICategoryRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCategoryCommandHandler(
            ICategoryRepository repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(
            CreateCategoryCommand request,
            CancellationToken cancellationToken)
        {
            var category = new Category(
                CategoryName.Create(request.Name));

            await _repository.AddAsync(category, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return category.Id;
        }
    }
}