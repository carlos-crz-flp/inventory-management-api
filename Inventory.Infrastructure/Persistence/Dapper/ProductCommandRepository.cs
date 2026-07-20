using Dapper;
using Inventory.Application.Abstractions.Persistence;
using Inventory.Domain.Aggregates.Products;

namespace Inventory.Infrastructure.Persistence.Dapper
{
    public sealed class ProductCommandRepository : IProductCommandRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public ProductCommandRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task AddAsync(
            Product product,
            CancellationToken cancellationToken)
        {
            const string sql = """
                INSERT INTO Products
                (
                    Id,
                    Sku,
                    Name,
                    CategoryId,
                    Stock,
                    IsActive
                )
                VALUES
                (
                    @Id,
                    @Sku,
                    @Name,
                    @CategoryId,
                    @Stock,
                    @IsActive
                );
                """;

            using var connection = _connectionFactory.CreateConnection();

            connection.Open();

            await connection.ExecuteAsync(
                new CommandDefinition(
                    sql,
                    new
                    {
                        product.Id,
                        Sku = product.Sku.Value,
                        Name = product.Name.Value,
                        product.CategoryId,
                        Stock = product.Stock.Value,
                        product.IsActive
                    },
                    cancellationToken: cancellationToken));
        }

        public async Task UpdateAsync(
            Product product,
            CancellationToken cancellationToken)
        {
            const string sql = """
                UPDATE Products
                SET
                    Sku = @Sku,
                    Name = @Name,
                    CategoryId = @CategoryId,
                    Stock = @Stock,
                    IsActive = @IsActive
                WHERE Id = @Id;
                """;

            using var connection = _connectionFactory.CreateConnection();

            connection.Open();

            await connection.ExecuteAsync(
                new CommandDefinition(
                    sql,
                    new
                    {
                        product.Id,
                        Sku = product.Sku.Value,
                        Name = product.Name.Value,
                        product.CategoryId,
                        Stock = product.Stock.Value,
                        product.IsActive
                    },
                    cancellationToken: cancellationToken));
        }

        public async Task DeleteAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            const string sql = """
                DELETE FROM Products
                WHERE Id = @Id;
                """;

            using var connection = _connectionFactory.CreateConnection();

            connection.Open();

            await connection.ExecuteAsync(
                new CommandDefinition(
                    sql,
                    new { Id = id },
                    cancellationToken: cancellationToken));
        }

        public async Task SaveStockMovementAsync(
            Product product,
            InventoryMovement movement,
            CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                const string updateProduct = """
                    UPDATE Products
                    SET Stock = @Stock
                    WHERE Id = @Id;
                    """;

                await connection.ExecuteAsync(
                    new CommandDefinition(
                        updateProduct,
                        new
                        {
                            product.Id,
                            Stock = product.Stock.Value
                        },
                        transaction: transaction,
                        cancellationToken: cancellationToken));

                const string insertMovement = """
                    INSERT INTO InventoryMovements
                    (
                        Id,
                        ProductId,
                        Type,
                        Quantity,
                        CreatedAt
                    )
                    VALUES
                    (
                        @Id,
                        @ProductId,
                        @Type,
                        @Quantity,
                        @CreatedAt
                    );
                    """;

                await connection.ExecuteAsync(
                    new CommandDefinition(
                        insertMovement,
                        new
                        {
                            movement.Id,
                            movement.ProductId,
                            Type = (int)movement.Type,
                            movement.Quantity,
                            movement.CreatedAt
                        },
                        transaction: transaction,
                        cancellationToken: cancellationToken));

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}