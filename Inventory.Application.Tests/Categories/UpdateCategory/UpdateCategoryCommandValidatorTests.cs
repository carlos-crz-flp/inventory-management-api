using Inventory.Application.Features.Categories.UpdateCategory;
using FluentAssertions;

namespace Inventory.Application.Tests.Categories.UpdateCategory
{
    public class UpdateCategoryCommandValidatorTests
    {
        private readonly UpdateCategoryCommandValidator _validator = new();

        [Fact]
        public void Should_Pass_When_Command_Is_Valid()
        {
            // Arrange
            var command = new UpdateCategoryCommand(
                Guid.NewGuid(),
                "Electronics");

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Should_Fail_When_Id_Is_Empty()
        {
            // Arrange
            var command = new UpdateCategoryCommand(
                Guid.Empty,
                "Electronics");

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
            var command = new UpdateCategoryCommand(
                Guid.NewGuid(),
                "");

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
            var command = new UpdateCategoryCommand(
                Guid.NewGuid(),
                new string('A', 101));

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();

            result.Errors.Should()
                .Contain(x => x.PropertyName == nameof(command.Name));
        }
    }
}