using Inventory.Application.Features.Products.UpdateProduct;
using FluentAssertions;

namespace Inventory.Application.Tests.Products.UpdateProduct
{
    public class UpdateProductCommandValidatorTests
    {
        private readonly UpdateProductCommandValidator _validator = new();

        [Fact]
        public void Should_Pass_When_Command_Is_Valid()
        {
            // Arrange
            var command = new UpdateProductCommand(
                Guid.NewGuid(),
                "Laptop",
                Guid.NewGuid());

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Should_Fail_When_Id_Is_Empty()
        {
            // Arrange
            var command = new UpdateProductCommand(
                Guid.Empty,
                "Laptop",
                Guid.NewGuid());

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();

            result.Errors.Should()
                .Contain(x => x.PropertyName == nameof(command.Id));
        }

        [Fact]
        public void Should_Fail_When_Name_Is_Empty()
        {
            // Arrange
            var command = new UpdateProductCommand(
                Guid.NewGuid(),
                "",
                Guid.NewGuid());

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();

            result.Errors.Should()
                .Contain(x => x.PropertyName == nameof(command.Name));
        }

        [Fact]
        public void Should_Fail_When_Name_Exceeds_Max_Length()
        {
            // Arrange
            var command = new UpdateProductCommand(
                Guid.NewGuid(),
                new string('A', 151),
                Guid.NewGuid());

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();

            result.Errors.Should()
                .Contain(x => x.PropertyName == nameof(command.Name));
        }

        [Fact]
        public void Should_Fail_When_CategoryId_Is_Empty()
        {
            // Arrange
            var command = new UpdateProductCommand(
                Guid.NewGuid(),
                "Laptop",
                Guid.Empty);

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();

            result.Errors.Should()
                .Contain(x => x.PropertyName == nameof(command.CategoryId));
        }
    }
}