using MediatR;

namespace Inventory.Application.Features.Categories.DeleteCategory
{
    public sealed record DeleteCategoryCommand(Guid Id) : IRequest;
}