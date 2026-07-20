using Inventory.Application.Abstractions.Persistence;
using Inventory.Infrastructure.Persistence;
using Inventory.Infrastructure.Persistence.Dapper;
using Inventory.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<InventoryDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<ICategoryCommandRepository, CategoryCommandRepository>();
            services.AddScoped<IProductCommandRepository, ProductCommandRepository>();

            services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}