using Inventory.Domain.Aggregates.Products;
using MediatR;

namespace Inventory.Application.Features.Products.GetProducts
{
    public sealed record GetProductsQuery
    : IRequest<IReadOnlyList<Product>>;
}