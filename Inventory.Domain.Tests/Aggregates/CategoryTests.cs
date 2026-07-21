using FluentAssertions;
using Inventory.Domain.Aggregates.Categories;
using Inventory.Domain.ValueObjects;

namespace Inventory.Domain.Tests.Aggregates
{
    public class CategoryTests
    {
        [Fact]
        public void Constructor_ShouldInitializeCategory()
        {
            // Arrange
            var name = CategoryName.Create("Electronics");

            // Act
            var category = new Category(name);

            // Assert
            category.Id.Should().NotBeEmpty();
            category.Name.Should().Be(name);
            category.IsActive.Should().BeTrue();
        }

        [Fact]
        public void Rename_ShouldUpdateName()
        {
            // Arrange
            var category = new Category(CategoryName.Create("Electronics"));
            var newName = CategoryName.Create("Computers");

            // Act
            category.Rename(newName);

            // Assert
            category.Name.Should().Be(newName);
        }

        [Fact]
        public void Rename_WithNull_ShouldThrowDomainException()
        {
            // Arrange
            var category = new Category(CategoryName.Create("Electronics"));

            // Act
            var action = () => category.Rename(null!);

            // Assert
            action.Should()
                .Throw<DomainException>()
                .WithMessage("Category name is required.");
        }

        [Fact]
        public void Deactivate_ShouldSetCategoryAsInactive()
        {
            // Arrange
            var category = new Category(CategoryName.Create("Electronics"));

            // Act
            category.Deactivate();

            // Assert
            category.IsActive.Should().BeFalse();
        }

        [Fact]
        public void Activate_ShouldSetCategoryAsActive()
        {
            // Arrange
            var category = new Category(CategoryName.Create("Electronics"));
            category.Deactivate();

            // Act
            category.Activate();

            // Assert
            category.IsActive.Should().BeTrue();
        }
    }
}