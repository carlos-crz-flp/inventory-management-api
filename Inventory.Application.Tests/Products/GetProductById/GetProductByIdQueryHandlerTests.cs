using Inventory.Application.Abstractions.Persistence;
using Inventory.Application.Features.Products.GetProductById;
using Inventory.Domain.Aggregates.Products;
using Inventory.Domain.ValueObjects;
using Moq;
using FluentAssertions;

namespace Inventory.Application.Tests.Products.GetProductById
{
    public class GetProductByIdQueryHandlerTests
    {
        private readonly Mock<IProductRepository> _repositoryMock;
        private readonly GetProductByIdQueryHandler _handler;

        public GetProductByIdQueryHandlerTests()
        {
            _repositoryMock = new Mock<IProductRepository>();

            _handler = new GetProductByIdQueryHandler(
                _repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_Product_When_It_Exists()
        {
            // Arrange
            var product = new Product(
                Sku.Create("SKU-001"),
                ProductName.Create("Laptop"),
                Guid.NewGuid());

            var query = new GetProductByIdQuery(product.Id);

            _repositoryMock
                .Setup(x => x.GetByIdAsync(
                    query.Id,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            // Act
            var result = await _handler.Handle(
                query,
                CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(product.Id);
            result.Sku.Value.Should().Be(product.Sku.Value);
            result.Name.Value.Should().Be(product.Name.Value);
            result.CategoryId.Should().Be(product.CategoryId);
            result.Stock.Value.Should().Be(product.Stock.Value);
            result.IsActive.Should().Be(product.IsActive);
        }

        [Fact]
        public async Task Handle_Should_Return_Null_When_Product_Does_Not_Exist()
        {
            // Arrange
            var query = new GetProductByIdQuery(Guid.NewGuid());

            _repositoryMock
                .Setup(x => x.GetByIdAsync(
                    query.Id,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product?)null);

            // Act
            var result = await _handler.Handle(
                query,
                CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }
    }
}