using Inventory.Domain.Aggregates.Categories;
using MediatR;

namespace Inventory.Application.Features.Categories.GetCategories
{
    public sealed record GetCategoriesQuery : IRequest<IReadOnlyList<Category>>;
}