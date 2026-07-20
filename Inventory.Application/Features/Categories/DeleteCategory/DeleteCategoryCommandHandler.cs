using Inventory.Application.Abstractions.Persistence;
using MediatR;

namespace Inventory.Application.Features.Categories.DeleteCategory
{
    public sealed class DeleteCategoryCommandHandler
    : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly ICategoryRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCategoryCommandHandler(
            ICategoryRepository repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(
            DeleteCategoryCommand request,
            CancellationToken cancellationToken)
        {
            var category = await _repository.GetByIdAsync(
                request.Id,
                cancellationToken);

            if (category is null)
                throw new KeyNotFoundException("Category not found.");

            category.Deactivate();

            _repository.Update(category);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}