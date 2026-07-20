using FluentValidation;

namespace Inventory.Application.Features.Products.CreateProduct
{
    public sealed class CreateProductCommandValidator
    : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Sku)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.CategoryId)
                .NotEmpty();
        }
    }
}