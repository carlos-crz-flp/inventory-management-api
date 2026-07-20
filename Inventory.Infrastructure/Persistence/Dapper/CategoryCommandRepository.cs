using Dapper;
using Inventory.Application.Abstractions.Persistence;
using Inventory.Domain.Aggregates.Categories;

namespace Inventory.Infrastructure.Persistence.Dapper
{
    public sealed class CategoryCommandRepository
        : ICategoryCommandRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public CategoryCommandRepository(
            IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task AddAsync(
            Category category,
            CancellationToken cancellationToken)
        {
            const string sql = """
                INSERT INTO Categories
                (
                    Id,
                    Name,
                    IsActive
                )
                VALUES
                (
                    @Id,
                    @Name,
                    @IsActive
                );
                """;

            using var connection = _connectionFactory.CreateConnection();

            await connection.ExecuteAsync(
                new CommandDefinition(
                    sql,
                    new
                    {
                        category.Id,
                        Name = category.Name.Value,
                        category.IsActive
                    },
                    cancellationToken: cancellationToken));
        }

        public async Task UpdateAsync(
            Category category,
            CancellationToken cancellationToken)
        {
            const string sql = """
                UPDATE Categories
                SET
                    Name = @Name,
                    IsActive = @IsActive
                WHERE Id = @Id;
                """;

            using var connection = _connectionFactory.CreateConnection();

            await connection.ExecuteAsync(
                new CommandDefinition(
                    sql,
                    new
                    {
                        category.Id,
                        Name = category.Name.Value,
                        category.IsActive
                    },
                    cancellationToken: cancellationToken));
        }

        public async Task DeleteAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            const string sql = """
                DELETE FROM Categories
                WHERE Id = @Id;
                """;

            using var connection = _connectionFactory.CreateConnection();

            await connection.ExecuteAsync(
                new CommandDefinition(
                    sql,
                    new { Id = id },
                    cancellationToken: cancellationToken));
        }
    }
}