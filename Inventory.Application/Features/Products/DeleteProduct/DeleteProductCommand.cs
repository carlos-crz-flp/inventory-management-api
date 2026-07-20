using MediatR;

namespace Inventory.Application.Features.Products.DeleteProduct
{
    public sealed record DeleteProductCommand(Guid Id)
    : IRequest;
}