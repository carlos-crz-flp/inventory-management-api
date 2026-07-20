using Inventory.Application.Abstractions.Persistence;
using Inventory.Domain.ValueObjects;
using MediatR;

namespace Inventory.Application.Features.Categories.UpdateCategory
{
    public sealed class UpdateCategoryCommandHandler
    : IRequestHandler<UpdateCategoryCommand>
    {
        private readonly ICategoryRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCategoryCommandHandler(
            ICategoryRepository repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(
            UpdateCategoryCommand request,
            CancellationToken cancellationToken)
        {
            var category = await _repository.GetByIdAsync(
                request.Id,
                cancellationToken);

            if (category is null)
                throw new KeyNotFoundException("Category not found.");

            category.Rename(CategoryName.Create(request.Name));

            _repository.Update(category);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}