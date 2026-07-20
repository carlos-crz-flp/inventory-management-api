using MediatR;

namespace Inventory.Application.Features.Products.IncreaseStock
{
    public sealed record IncreaseStockCommand(
    Guid ProductId,
    int Quantity
) : IRequest;
}