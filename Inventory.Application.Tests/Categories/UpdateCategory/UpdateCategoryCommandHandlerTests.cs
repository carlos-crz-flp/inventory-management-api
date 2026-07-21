using FluentAssertions;
using Inventory.Application.Abstractions.Persistence;
using Inventory.Application.Features.Categories.UpdateCategory;
using Inventory.Domain.Aggregates.Categories;
using Inventory.Domain.ValueObjects;
using Moq;

namespace Inventory.Application.Tests.Categories.UpdateCategory
{
    public class UpdateCategoryCommandHandlerTests
    {
        private readonly Mock<ICategoryRepository> _queryRepositoryMock;
        private readonly Mock<ICategoryCommandRepository> _commandRepositoryMock;
        private readonly UpdateCategoryCommandHandler _handler;

        public UpdateCategoryCommandHandlerTests()
        {
            _queryRepositoryMock = new Mock<ICategoryRepository>();
            _commandRepositoryMock = new Mock<ICategoryCommandRepository>();

            _handler = new UpdateCategoryCommandHandler(
                _queryRepositoryMock.Object,
                _commandRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Update_Category()
        {
            // Arrange
            var category = new Category(
                CategoryName.Create("Old Name"));

            var command = new UpdateCategoryCommand(
                category.Id,
                "New Name");

            _queryRepositoryMock
                .Setup(x => x.GetByIdAsync(
                    command.Id,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(category);

            _commandRepositoryMock
                .Setup(x => x.UpdateAsync(
                    It.IsAny<Category>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(
                command,
                CancellationToken.None);

            // Assert
            category.Name.Value.Should().Be("New Name");

            _commandRepositoryMock.Verify(x =>
                x.UpdateAsync(
                    category,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_When_Category_Does_Not_Exist()
        {
            // Arrange
            var command = new UpdateCategoryCommand(
                Guid.NewGuid(),
                "Electronics");

            _queryRepositoryMock
                .Setup(x => x.GetByIdAsync(
                    command.Id,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Category?)null);

            // Act
            var action = async () =>
                await _handler.Handle(
                    command,
                    CancellationToken.None);

            // Assert
            await action.Should()
                .ThrowAsync<KeyNotFoundException>()
                .WithMessage("Category not found.");

            _commandRepositoryMock.Verify(x =>
                x.UpdateAsync(
                    It.IsAny<Category>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}