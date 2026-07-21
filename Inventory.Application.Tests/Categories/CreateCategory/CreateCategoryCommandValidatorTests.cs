using Inventory.Application.Features.Categories.CreateCategory;
using FluentAssertions;

namespace Inventory.Application.Tests.Categories.CreateCategory
{
    public class CreateCategoryCommandValidatorTests
    {
        private readonly CreateCategoryCommandValidator _validator = new();

        [Fact]
        public void Should_Pass_When_Command_Is_Valid()
        {
            // Arrange
            var command = new CreateCategoryCommand("Electronics");

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Should_Fail_When_Name_Is_Empty()
        {
            // Arrange
            var command = new CreateCategoryCommand("");

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
            var command = new CreateCategoryCommand(new string('A', 101));

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();

            result.Errors.Should()
                .Contain(x => x.PropertyName == nameof(command.Name));
        }
    }
}