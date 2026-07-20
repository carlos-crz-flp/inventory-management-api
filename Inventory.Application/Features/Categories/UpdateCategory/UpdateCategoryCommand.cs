using MediatR;

namespace Inventory.Application.Features.Categories.UpdateCategory
{
    public sealed record UpdateCategoryCommand(
    Guid Id,
    string Name) : IRequest;
}