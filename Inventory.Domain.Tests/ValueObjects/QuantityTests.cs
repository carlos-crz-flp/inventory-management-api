using FluentAssertions;
using Inventory.Domain.ValueObjects;

namespace Inventory.Domain.Tests.ValueObjects
{
    public class QuantityTests
    {
        [Fact]
        public void Create_WithValidValue_ShouldCreateQuantity()
        {
            // Arrange
            const int value = 10;

            // Act
            var quantity = Quantity.Create(value);

            // Assert
            quantity.Value.Should().Be(value);
        }

        [Fact]
        public void Create_WithZero_ShouldCreateQuantity()
        {
            // Act
            var quantity = Quantity.Create(0);

            // Assert
            quantity.Value.Should().Be(0);
        }

        [Fact]
        public void Create_WithNegativeValue_ShouldThrowDomainException()
        {
            // Act
            var action = () => Quantity.Create(-1);

            // Assert
            action.Should()
                .Throw<DomainException>()
                .WithMessage("Quantity cannot be negative.");
        }

        [Fact]
        public void Add_WithPositiveValue_ShouldIncreaseQuantity()
        {
            // Arrange
            var quantity = Quantity.Create(10);

            // Act
            var result = quantity.Add(5);

            // Assert
            result.Value.Should().Be(15);
        }

        [Fact]
        public void Add_WithZero_ShouldThrowDomainException()
        {
            // Arrange
            var quantity = Quantity.Create(10);

            // Act
            var action = () => quantity.Add(0);

            // Assert
            action.Should()
                .Throw<DomainException>()
                .WithMessage("Quantity to add must be greater than zero.");
        }

        [Fact]
        public void Add_WithNegativeValue_ShouldThrowDomainException()
        {
            // Arrange
            var quantity = Quantity.Create(10);

            // Act
            var action = () => quantity.Add(-2);

            // Assert
            action.Should()
                .Throw<DomainException>()
                .WithMessage("Quantity to add must be greater than zero.");
        }

        [Fact]
        public void Subtract_WithValidValue_ShouldDecreaseQuantity()
        {
            // Arrange
            var quantity = Quantity.Create(10);

            // Act
            var result = quantity.Subtract(4);

            // Assert
            result.Value.Should().Be(6);
        }

        [Fact]
        public void Subtract_AllStock_ShouldReturnZero()
        {
            // Arrange
            var quantity = Quantity.Create(10);

            // Act
            var result = quantity.Subtract(10);

            // Assert
            result.Value.Should().Be(0);
        }

        [Fact]
        public void Subtract_WithZero_ShouldThrowDomainException()
        {
            // Arrange
            var quantity = Quantity.Create(10);

            // Act
            var action = () => quantity.Subtract(0);

            // Assert
            action.Should()
                .Throw<DomainException>()
                .WithMessage("Quantity to subtract must be greater than zero.");
        }

        [Fact]
        public void Subtract_WithNegativeValue_ShouldThrowDomainException()
        {
            // Arrange
            var quantity = Quantity.Create(10);

            // Act
            var action = () => quantity.Subtract(-5);

            // Assert
            action.Should()
                .Throw<DomainException>()
                .WithMessage("Quantity to subtract must be greater than zero.");
        }

        [Fact]
        public void Subtract_WithInsufficientStock_ShouldThrowDomainException()
        {
            // Arrange
            var quantity = Quantity.Create(5);

            // Act
            var action = () => quantity.Subtract(6);

            // Assert
            action.Should()
                .Throw<DomainException>()
                .WithMessage("Insufficient stock.");
        }
    }
}