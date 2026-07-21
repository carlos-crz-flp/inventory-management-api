using FluentAssertions;
using Inventory.Domain.ValueObjects;

namespace Inventory.Domain.Tests.ValueObjects
{
    public class SkuTests
    {
        [Fact]
        public void Create_WithValidValue_ShouldCreateSku()
        {
            var sku = Sku.Create("ABC-001");

            sku.Value.Should().Be("ABC-001");
        }

        [Fact]
        public void Create_ShouldTrimAndConvertToUppercase()
        {
            var sku = Sku.Create("  abc-001  ");

            sku.Value.Should().Be("ABC-001");
        }

        [Fact]
        public void Create_WithNull_ShouldThrowDomainException()
        {
            var action = () => Sku.Create(null!);

            action.Should()
                .Throw<DomainException>()
                .WithMessage("SKU is required.");
        }

        [Fact]
        public void Create_WithEmptyValue_ShouldThrowDomainException()
        {
            var action = () => Sku.Create("");

            action.Should()
                .Throw<DomainException>()
                .WithMessage("SKU is required.");
        }

        [Fact]
        public void Create_WithMoreThan50Characters_ShouldThrowDomainException()
        {
            var value = new string('A', 51);

            var action = () => Sku.Create(value);

            action.Should()
                .Throw<DomainException>()
                .WithMessage("SKU cannot exceed 50 characters.");
        }

        [Fact]
        public void EqualValues_ShouldBeEqual()
        {
            var first = Sku.Create("ABC-001");
            var second = Sku.Create("abc-001");

            first.Should().Be(second);
        }

        [Fact]
        public void DifferentValues_ShouldNotBeEqual()
        {
            var first = Sku.Create("ABC-001");
            var second = Sku.Create("ABC-002");

            first.Should().NotBe(second);
        }
    }
}