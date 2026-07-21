using FluentAssertions;
using Inventory.Application.Abstractions.Persistence;
using Inventory.Application.Features.Categories.DeleteCategory;
using Inventory.Domain.Aggregates.Categories;
using Inventory.Domain.ValueObjects;
using Moq;

namespace Inventory.Application.Tests.Categories.DeleteCategory
{
    public class DeleteCategoryCommandHandlerTests
    {
        private readonly Mock<ICategoryRepository> _queryRepositoryMock;
        private readonly Mock<ICategoryCommandRepository> _commandRepositoryMock;
        private readonly DeleteCategoryCommandHandler _handler;

        public DeleteCategoryCommandHandlerTests()
        {
            _queryRepositoryMock = new Mock<ICategoryRepository>();
            _commandRepositoryMock = new Mock<ICategoryCommandRepository>();

            _handler = new DeleteCategoryCommandHandler(
                _queryRepositoryMock.Object,
                _commandRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Deactivate_Category()
        {
            // Arrange
            var category = new Category(
                CategoryName.Create("Electronics"));

            var command = new DeleteCategoryCommand(category.Id);

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
            category.IsActive.Should().BeFalse();

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
            var command = new DeleteCategoryCommand(Guid.NewGuid());

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