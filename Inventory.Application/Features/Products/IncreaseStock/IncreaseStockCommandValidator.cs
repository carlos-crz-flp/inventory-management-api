using FluentValidation;

namespace Inventory.Application.Features.Products.IncreaseStock
{
    public sealed class IncreaseStockCommandValidator
        : AbstractValidator<IncreaseStockCommand>
    {
        public IncreaseStockCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty();

            RuleFor(x => x.Quantity)
                .GreaterThan(0);
        }
    }
}