using Inventory.Domain.Aggregates.Categories;
using MediatR;

namespace Inventory.Application.Features.Categories.GetCategoryById
{
    public sealed record GetCategoryByIdQuery(Guid Id) : IRequest<Category?>;
}