using MediatR;

namespace Inventory.Application.Features.Products.UpdateProduct
{
    public sealed record UpdateProductCommand(
    Guid Id,
    string Name,
    Guid CategoryId
) : IRequest;
}