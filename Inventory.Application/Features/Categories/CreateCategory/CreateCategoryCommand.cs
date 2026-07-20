using MediatR;

namespace Inventory.Application.Features.Categories.CreateCategory
{
    public sealed record CreateCategoryCommand(string Name) : IRequest<Guid>;
}