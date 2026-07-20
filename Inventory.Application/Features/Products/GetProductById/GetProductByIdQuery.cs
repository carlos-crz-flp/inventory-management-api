using Inventory.Domain.Aggregates.Products;
using MediatR;

namespace Inventory.Application.Features.Products.GetProductById
{
    public sealed record GetProductByIdQuery(Guid Id)
    : IRequest<Product?>;
}