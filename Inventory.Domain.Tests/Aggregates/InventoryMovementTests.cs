using Inventory.Domain.Aggregates.Products;
using Inventory.Domain.Enums;
using FluentAssertions;

namespace Inventory.Domain.Tests.Aggregates
{
    public class InventoryMovementTests
    {
        [Fact]
        public void Constructor_ShouldInitializeMovement()
        {
            // Arrange
            var productId = Guid.NewGuid();

            // Act
            var movement = new InventoryMovement(
                productId,
                InventoryMovementType.Entry,
                10);

            // Assert
            movement.Id.Should().NotBeEmpty();
            movement.ProductId.Should().Be(productId);
            movement.Type.Should().Be(InventoryMovementType.Entry);
            movement.Quantity.Should().Be(10);
            movement.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
        }

        [Fact]
        public void Constructor_ShouldCreateEntryMovement()
        {
            // Arrange
            var productId = Guid.NewGuid();

            // Act
            var movement = new InventoryMovement(
                productId,
                InventoryMovementType.Entry,
                5);

            // Assert
            movement.Type.Should().Be(InventoryMovementType.Entry);
        }

        [Fact]
        public void Constructor_ShouldCreateExitMovement()
        {
            // Arrange
            var productId = Guid.NewGuid();

            // Act
            var movement = new InventoryMovement(
                productId,
                InventoryMovementType.Exit,
                3);

            // Assert
            movement.Type.Should().Be(InventoryMovementType.Exit);
        }

        [Fact]
        public void Constructor_ShouldAssignQuantity()
        {
            // Arrange
            var productId = Guid.NewGuid();

            // Act
            var movement = new InventoryMovement(
                productId,
                InventoryMovementType.Entry,
                25);

            // Assert
            movement.Quantity.Should().Be(25);
        }

        [Fact]
        public void Constructor_ShouldAssignProductId()
        {
            // Arrange
            var productId = Guid.NewGuid();

            // Act
            var movement = new InventoryMovement(
                productId,
                InventoryMovementType.Exit,
                7);

            // Assert
            movement.ProductId.Should().Be(productId);
        }
    }
}