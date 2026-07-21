using FluentAssertions;
using Inventory.Application.Abstractions.Persistence;
using Inventory.Application.Features.Products.DecreaseStock;
using Inventory.Domain.Aggregates.Products;
using Inventory.Domain.ValueObjects;
using Moq;

namespace Inventory.Application.Tests.Products.DecreaseStock
{
    public class DecreaseStockCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _repositoryMock;
        private readonly Mock<IProductCommandRepository> _commandRepositoryMock;
        private readonly DecreaseStockCommandHandler _handler;

        public DecreaseStockCommandHandlerTests()
        {
            _repositoryMock = new Mock<IProductRepository>();
            _commandRepositoryMock = new Mock<IProductCommandRepository>();

            _handler = new DecreaseStockCommandHandler(
                _repositoryMock.Object,
                _commandRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Decrease_Stock_And_Save_Movement()
        {
            // Arrange
            var product = new Product(
                Sku.Create("SKU-001"),
                ProductName.Create("Laptop"),
                Guid.NewGuid());

            product.IncreaseStock(20);

            var command = new DecreaseStockCommand(
                product.Id,
                5);

            _repositoryMock
                .Setup(x => x.GetByIdAsync(
                    command.ProductId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            // Act
            await _handler.Handle(
                command,
                CancellationToken.None);

            // Assert
            product.Stock.Value.Should().Be(15);

            product.LastMovement.Should().NotBeNull();

            _commandRepositoryMock.Verify(x =>
                x.SaveStockMovementAsync(
                    product,
                    product.LastMovement!,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_When_Product_Does_Not_Exist()
        {
            // Arrange
            var command = new DecreaseStockCommand(
                Guid.NewGuid(),
                5);

            _repositoryMock
                .Setup(x => x.GetByIdAsync(
                    command.ProductId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product?)null);

            // Act
            Func<Task> act = async () =>
                await _handler.Handle(
                    command,
                    CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<KeyNotFoundException>()
                .WithMessage("Product not found.");

            _commandRepositoryMock.Verify(x =>
                x.SaveStockMovementAsync(
                    It.IsAny<Product>(),
                    It.IsAny<InventoryMovement>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}