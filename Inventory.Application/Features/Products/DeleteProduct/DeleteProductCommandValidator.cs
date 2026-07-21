using FluentValidation;

namespace Inventory.Application.Features.Products.DeleteProduct
{
    public sealed class DeleteProductCommandValidator
        : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }
}