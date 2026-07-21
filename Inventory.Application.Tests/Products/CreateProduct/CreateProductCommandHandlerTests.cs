using FluentAssertions;
using Inventory.Application.Abstractions.Persistence;
using Inventory.Application.Features.Products.CreateProduct;
using Inventory.Domain.Aggregates.Products;
using Moq;

namespace Inventory.Application.Tests.Products.CreateProduct
{
    public class CreateProductCommandHandlerTests
    {
        private readonly Mock<IProductCommandRepository> _repositoryMock;
        private readonly CreateProductCommandHandler _handler;

        public CreateProductCommandHandlerTests()
        {
            _repositoryMock = new Mock<IProductCommandRepository>();

            _handler = new CreateProductCommandHandler(
                _repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Create_Product_And_Return_Id()
        {
            // Arrange
            var command = new CreateProductCommand(
                "SKU-001",
                "Laptop",
                Guid.NewGuid());

            Product? capturedProduct = null;

            _repositoryMock
                .Setup(x => x.AddAsync(
                    It.IsAny<Product>(),
                    It.IsAny<CancellationToken>()))
                .Callback<Product, CancellationToken>((product, _) =>
                {
                    capturedProduct = product;
                })
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(
                command,
                CancellationToken.None);

            // Assert
            result.Should().NotBeEmpty();

            capturedProduct.Should().NotBeNull();

            capturedProduct!.Sku.Value.Should().Be(command.Sku);
            capturedProduct.Name.Value.Should().Be(command.Name);
            capturedProduct.CategoryId.Should().Be(command.CategoryId);
            capturedProduct.IsActive.Should().BeTrue();

            _repositoryMock.Verify(x =>
                x.AddAsync(
                    It.IsAny<Product>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}