using FluentAssertions;
using Inventory.Domain.ValueObjects;

namespace Inventory.Domain.Tests.ValueObjects
{
    public class ProductNameTests
    {
        [Fact]
        public void Create_WithValidValue_ShouldCreateProductName()
        {
            var name = ProductName.Create("Laptop");

            name.Value.Should().Be("Laptop");
        }

        [Fact]
        public void Create_ShouldTrimWhitespace()
        {
            var name = ProductName.Create("  Laptop  ");

            name.Value.Should().Be("Laptop");
        }

        [Fact]
        public void Create_WithNull_ShouldThrowDomainException()
        {
            var action = () => ProductName.Create(null!);

            action.Should()
                .Throw<DomainException>()
                .WithMessage("Product name is required.");
        }

        [Fact]
        public void Create_WithEmptyValue_ShouldThrowDomainException()
        {
            var action = () => ProductName.Create("");

            action.Should()
                .Throw<DomainException>()
                .WithMessage("Product name is required.");
        }

        [Fact]
        public void Create_WithMoreThan150Characters_ShouldThrowDomainException()
        {
            var value = new string('A', 151);

            var action = () => ProductName.Create(value);

            action.Should()
                .Throw<DomainException>()
                .WithMessage("Product name cannot exceed 150 characters.");
        }

        [Fact]
        public void EqualValues_ShouldBeEqual()
        {
            var first = ProductName.Create("Laptop");
            var second = ProductName.Create("Laptop");

            first.Should().Be(second);
        }

        [Fact]
        public void DifferentValues_ShouldNotBeEqual()
        {
            var first = ProductName.Create("Laptop");
            var second = ProductName.Create("Mouse");

            first.Should().NotBe(second);
        }
    }
}