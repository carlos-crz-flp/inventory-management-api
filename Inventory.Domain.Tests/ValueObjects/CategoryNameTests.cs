using FluentAssertions;
using Inventory.Domain.ValueObjects;

namespace Inventory.Domain.Tests.ValueObjects
{
    public class CategoryNameTests
    {
        [Fact]
        public void Create_WithValidValue_ShouldCreateCategoryName()
        {
            var name = CategoryName.Create("Electronics");

            name.Value.Should().Be("Electronics");
        }

        [Fact]
        public void Create_ShouldTrimWhitespace()
        {
            var name = CategoryName.Create("  Electronics  ");

            name.Value.Should().Be("Electronics");
        }

        [Fact]
        public void Create_WithNull_ShouldThrowDomainException()
        {
            var action = () => CategoryName.Create(null!);

            action.Should()
                .Throw<DomainException>()
                .WithMessage("Category name is required.");
        }

        [Fact]
        public void Create_WithEmptyValue_ShouldThrowDomainException()
        {
            var action = () => CategoryName.Create("");

            action.Should()
                .Throw<DomainException>()
                .WithMessage("Category name is required.");
        }

        [Fact]
        public void Create_WithMoreThan100Characters_ShouldThrowDomainException()
        {
            var value = new string('A', 101);

            var action = () => CategoryName.Create(value);

            action.Should()
                .Throw<DomainException>()
                .WithMessage("Category name cannot exceed 100 characters.");
        }

        [Fact]
        public void EqualValues_ShouldBeEqual()
        {
            var first = CategoryName.Create("Books");
            var second = CategoryName.Create("Books");

            first.Should().Be(second);
        }

        [Fact]
        public void DifferentValues_ShouldNotBeEqual()
        {
            var first = CategoryName.Create("Books");
            var second = CategoryName.Create("Food");

            first.Should().NotBe(second);
        }
    }
}