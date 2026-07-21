using FluentAssertions;
using Inventory.Application.Abstractions.Persistence;
using Inventory.Application.Features.Categories.CreateCategory;
using Inventory.Domain.Aggregates.Categories;
using Moq;

namespace Inventory.Application.Tests.Categories.CreateCategory
{
    public class CreateCategoryCommandHandlerTests
    {
        private readonly Mock<ICategoryCommandRepository> _repositoryMock;
        private readonly CreateCategoryCommandHandler _handler;

        public CreateCategoryCommandHandlerTests()
        {
            _repositoryMock = new Mock<ICategoryCommandRepository>();

            _handler = new CreateCategoryCommandHandler(
                _repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Create_Category_And_Return_Id()
        {
            // Arrange
            var command = new CreateCategoryCommand("Electronics");

            Category? capturedCategory = null;

            _repositoryMock
                .Setup(x => x.AddAsync(
                    It.IsAny<Category>(),
                    It.IsAny<CancellationToken>()))
                .Callback<Category, CancellationToken>((category, _) =>
                {
                    capturedCategory = category;
                })
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(
                command,
                CancellationToken.None);

            // Assert
            result.Should().NotBeEmpty();

            capturedCategory.Should().NotBeNull();
            capturedCategory!.Name.Value.Should().Be("Electronics");
            capturedCategory.IsActive.Should().BeTrue();

            _repositoryMock.Verify(x =>
                x.AddAsync(
                    It.IsAny<Category>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}