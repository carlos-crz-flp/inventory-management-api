using FluentValidation;
using Inventory.Application.Features.Categories.CreateCategory;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

            services.AddValidatorsFromAssemblyContaining<CreateCategoryCommandValidator>();

            return services;
        }
    }
}