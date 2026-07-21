using FluentAssertions;
using Inventory.Application.Abstractions.Persistence;
using Inventory.Application.Features.Products.DeleteProduct;
using Inventory.Domain.Aggregates.Products;
using Inventory.Domain.ValueObjects;
using Moq;

namespace Inventory.Application.Tests.Products.DeleteProduct
{
    public class DeleteProductCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _repositoryMock;
        private readonly Mock<IProductCommandRepository> _commandRepositoryMock;
        private readonly DeleteProductCommandHandler _handler;

        public DeleteProductCommandHandlerTests()
        {
            _repositoryMock = new Mock<IProductRepository>();
            _commandRepositoryMock = new Mock<IProductCommandRepository>();

            _handler = new DeleteProductCommandHandler(
                _repositoryMock.Object,
                _commandRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Deactivate_Product()
        {
            // Arrange
            var product = new Product(
                Sku.Create("SKU-001"),
                ProductName.Create("Laptop"),
                Guid.NewGuid());

            var command = new DeleteProductCommand(product.Id);

            _repositoryMock
                .Setup(x => x.GetByIdAsync(
                    command.Id,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            // Act
            await _handler.Handle(
                command,
                CancellationToken.None);

            // Assert
            product.IsActive.Should().BeFalse();

            _commandRepositoryMock.Verify(x =>
                x.UpdateAsync(
                    product,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_When_Product_Does_Not_Exist()
        {
            // Arrange
            var command = new DeleteProductCommand(Guid.NewGuid());

            _repositoryMock
                .Setup(x => x.GetByIdAsync(
                    command.Id,
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
                x.UpdateAsync(
                    It.IsAny<Product>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}