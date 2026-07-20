using MediatR;

namespace Inventory.Application.Features.Products.DecreaseStock
{
    public sealed record DecreaseStockCommand(
    Guid ProductId,
    int Quantity
) : IRequest;
}