using MediatR;

namespace Inventory.Application.Features.Products.CreateProduct
{
    public sealed record CreateProductCommand(
    string Sku,
    string Name,
    Guid CategoryId
) : IRequest<Guid>;
}