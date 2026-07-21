using FluentValidation;

namespace Inventory.Application.Features.Products.DecreaseStock
{
    public sealed class DecreaseStockCommandValidator
        : AbstractValidator<DecreaseStockCommand>
    {
        public DecreaseStockCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty();

            RuleFor(x => x.Quantity)
                .GreaterThan(0);
        }
    }
}