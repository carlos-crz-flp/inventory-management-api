using Inventory.Application.Features.Products.DeleteProduct;
using FluentAssertions;

namespace Inventory.Application.Tests.Products.DeleteProduct
{
    public class DeleteProductCommandValidatorTests
    {
        private readonly DeleteProductCommandValidator _validator = new();

        [Fact]
        public void Should_Pass_When_Command_Is_Valid()
        {
            // Arrange
            var command = new DeleteProductCommand(Guid.NewGuid());

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Should_Fail_When_Id_Is_Empty()
        {
            // Arrange
            var command = new DeleteProductCommand(Guid.Empty);

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();

            result.Errors.Should()
                .Contain(x => x.PropertyName == nameof(command.Id));
        }
    }
}