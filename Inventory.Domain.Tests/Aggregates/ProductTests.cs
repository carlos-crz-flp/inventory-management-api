using FluentAssertions;
using Inventory.Domain.Aggregates.Products;
using Inventory.Domain.Enums;
using Inventory.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Tests.Aggregates
{
    public class ProductTests
    {
        [Fact]
        public void Constructor_ShouldInitializeProduct()
        {
            // Arrange
            var sku = Sku.Create("SKU-001");
            var name = ProductName.Create("Laptop");
            var categoryId = Guid.NewGuid();

            // Act
            var product = new Product(sku, name, categoryId);

            // Assert
            product.Id.Should().NotBeEmpty();
            product.Sku.Should().Be(sku);
            product.Name.Should().Be(name);
            product.CategoryId.Should().Be(categoryId);
            product.Stock.Value.Should().Be(0);
            product.IsActive.Should().BeTrue();
            product.Movements.Should().BeEmpty();
            product.LastMovement.Should().BeNull();
        }

        [Fact]
        public void Rename_ShouldUpdateName()
        {
            // Arrange
            var product = CreateProduct();
            var newName = ProductName.Create("Gaming Laptop");

            // Act
            product.Rename(newName);

            // Assert
            product.Name.Should().Be(newName);
        }

        [Fact]
        public void Rename_WithNull_ShouldThrowDomainException()
        {
            // Arrange
            var product = CreateProduct();

            // Act
            var action = () => product.Rename(null!);

            // Assert
            action.Should()
                .Throw<DomainException>()
                .WithMessage("Product name is required.");
        }

        [Fact]
        public void ChangeCategory_ShouldUpdateCategoryId()
        {
            // Arrange
            var product = CreateProduct();
            var categoryId = Guid.NewGuid();

            // Act
            product.ChangeCategory(categoryId);

            // Assert
            product.CategoryId.Should().Be(categoryId);
        }

        [Fact]
        public void IncreaseStock_ShouldIncreaseStock()
        {
            // Arrange
            var product = CreateProduct();

            // Act
            product.IncreaseStock(10);

            // Assert
            product.Stock.Value.Should().Be(10);
        }

        [Fact]
        public void IncreaseStock_ShouldRegisterEntryMovement()
        {
            // Arrange
            var product = CreateProduct();

            // Act
            product.IncreaseStock(10);

            // Assert
            product.Movements.Should().HaveCount(1);

            product.LastMovement.Should().NotBeNull();
            product.LastMovement!.Type.Should().Be(InventoryMovementType.Entry);
            product.LastMovement.Quantity.Should().Be(10);
            product.LastMovement.ProductId.Should().Be(product.Id);
        }

        [Fact]
        public void IncreaseStock_Twice_ShouldKeepMovementHistory()
        {
            // Arrange
            var product = CreateProduct();

            // Act
            product.IncreaseStock(10);
            product.IncreaseStock(5);

            // Assert
            product.Stock.Value.Should().Be(15);
            product.Movements.Should().HaveCount(2);
            product.LastMovement!.Quantity.Should().Be(5);
        }

        [Fact]
        public void IncreaseStock_WithNegativeQuantity_ShouldThrowDomainException()
        {
            // Arrange
            var product = CreateProduct();

            // Act
            var action = () => product.IncreaseStock(-5);

            // Assert
            action.Should()
                .Throw<DomainException>()
                .WithMessage("Quantity to add must be greater than zero.");
        }

        [Fact]
        public void DecreaseStock_ShouldDecreaseStock()
        {
            // Arrange
            var product = CreateProduct();
            product.IncreaseStock(20);

            // Act
            product.DecreaseStock(5);

            // Assert
            product.Stock.Value.Should().Be(15);
        }

        [Fact]
        public void DecreaseStock_ShouldRegisterExitMovement()
        {
            // Arrange
            var product = CreateProduct();
            product.IncreaseStock(20);

            // Act
            product.DecreaseStock(5);

            // Assert
            product.LastMovement.Should().NotBeNull();
            product.LastMovement!.Type.Should().Be(InventoryMovementType.Exit);
            product.LastMovement.Quantity.Should().Be(5);
        }

        [Fact]
        public void DecreaseStock_WithInsufficientStock_ShouldThrowDomainException()
        {
            // Arrange
            var product = CreateProduct();

            // Act
            var action = () => product.DecreaseStock(1);

            // Assert
            action.Should()
                .Throw<DomainException>()
                .WithMessage("Insufficient stock.");
        }

        [Fact]
        public void DecreaseStock_WithNegativeQuantity_ShouldThrowDomainException()
        {
            // Arrange
            var product = CreateProduct();
            product.IncreaseStock(10);

            // Act
            var action = () => product.DecreaseStock(-2);

            // Assert
            action.Should()
                .Throw<DomainException>()
                .WithMessage("Quantity to subtract must be greater than zero.");
        }

        [Fact]
        public void Activate_ShouldSetProductAsActive()
        {
            // Arrange
            var product = CreateProduct();
            product.Deactivate();

            // Act
            product.Activate();

            // Assert
            product.IsActive.Should().BeTrue();
        }

        [Fact]
        public void Deactivate_ShouldSetProductAsInactive()
        {
            // Arrange
            var product = CreateProduct();

            // Act
            product.Deactivate();

            // Assert
            product.IsActive.Should().BeFalse();
        }

        private static Product CreateProduct()
        {
            return new Product(
                Sku.Create("SKU-001"),
                ProductName.Create("Laptop"),
                Guid.NewGuid());
        }
    }
}