using FluentValidation;

namespace Inventory.Application.Features.Products.UpdateProduct
{
    public sealed class UpdateProductCommandValidator
        : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.CategoryId)
                .NotEmpty();
        }
    }
}