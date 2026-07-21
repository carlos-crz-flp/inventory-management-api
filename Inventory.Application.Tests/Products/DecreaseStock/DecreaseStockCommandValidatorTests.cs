using Inventory.Application.Features.Products.DecreaseStock;
using FluentAssertions;

namespace Inventory.Application.Tests.Products.DecreaseStock
{
    public class DecreaseStockCommandValidatorTests
    {
        private readonly DecreaseStockCommandValidator _validator = new();

        [Fact]
        public void Should_Pass_When_Command_Is_Valid()
        {
            // Arrange
            var command = new DecreaseStockCommand(
                Guid.NewGuid(),
                5);

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Should_Fail_When_ProductId_Is_Empty()
        {
            // Arrange
            var command = new DecreaseStockCommand(
                Guid.Empty,
                5);

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();

            result.Errors.Should()
                .Contain(x => x.PropertyName == nameof(command.ProductId));
        }

        [Fact]
        public void Should_Fail_When_Quantity_Is_Less_Than_One()
        {
            // Arrange
            var command = new DecreaseStockCommand(
                Guid.NewGuid(),
                0);

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();

            result.Errors.Should()
                .Contain(x => x.PropertyName == nameof(command.Quantity));
        }
    }
}