using FluentAssertions;
using Inventory.Application.Features.Products.CreateProduct;

namespace Inventory.Application.Tests.Products.CreateProduct
{
    public class CreateProductCommandValidatorTests
    {
        private readonly CreateProductCommandValidator _validator = new();

        [Fact]
        public void Should_Pass_When_Command_Is_Valid()
        {
            // Arrange
            var command = new CreateProductCommand(
                "SKU-001",
                "Laptop",
                Guid.NewGuid());

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Should_Fail_When_Sku_Is_Empty()
        {
            // Arrange
            var command = new CreateProductCommand(
                "",
                "Laptop",
                Guid.NewGuid());

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();

            result.Errors.Should()
                .Contain(x => x.PropertyName == nameof(command.Sku));
        }

        [Fact]
        public void Should_Fail_When_Sku_Exceeds_Max_Length()
        {
            // Arrange
            var command = new CreateProductCommand(
                new string('A', 51),
                "Laptop",
                Guid.NewGuid());

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();

            result.Errors.Should()
                .Contain(x => x.PropertyName == nameof(command.Sku));
        }

        [Fact]
        public void Should_Fail_When_Name_Is_Empty()
        {
            // Arrange
            var command = new CreateProductCommand(
                "SKU-001",
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
            var command = new CreateProductCommand(
                "SKU-001",
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
            var command = new CreateProductCommand(
                "SKU-001",
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