using FluentAssertions;
using Inventory.Application.Abstractions.Persistence;
using Inventory.Application.Features.Products.UpdateProduct;
using Inventory.Domain.Aggregates.Products;
using Inventory.Domain.ValueObjects;
using Moq;

namespace Inventory.Application.Tests.Products.UpdateProduct
{
    public class UpdateProductCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _repositoryMock;
        private readonly Mock<IProductCommandRepository> _commandRepositoryMock;
        private readonly UpdateProductCommandHandler _handler;

        public UpdateProductCommandHandlerTests()
        {
            _repositoryMock = new Mock<IProductRepository>();
            _commandRepositoryMock = new Mock<IProductCommandRepository>();

            _handler = new UpdateProductCommandHandler(
                _repositoryMock.Object,
                _commandRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Update_Product()
        {
            // Arrange
            var originalCategoryId = Guid.NewGuid();
            var newCategoryId = Guid.NewGuid();

            var product = new Product(
                Sku.Create("SKU-001"),
                ProductName.Create("Old Product"),
                originalCategoryId);

            var command = new UpdateProductCommand(
                product.Id,
                "New Product",
                newCategoryId);

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
            product.Name.Value.Should().Be(command.Name);
            product.CategoryId.Should().Be(command.CategoryId);

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
            var command = new UpdateProductCommand(
                Guid.NewGuid(),
                "Laptop",
                Guid.NewGuid());

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