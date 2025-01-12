using BookStoreApp.Application.Commands;
using BookStoreApp.Application.Exceptions;
using BookStoreApp.Application.Repositories;
using BookStoreApp.Data;
using Moq;

namespace BookStoreApp.Tests;

public class BuyCommandHandlerTests
{
    [Fact]
    public async Task ExecuteAsync_WithNullId_ShouldThrowBookIdNotSetException()
    {
        // Arrange
        var mockRepository = new Mock<IBookRepository>();
        var handler = new BuyCommandHandler(mockRepository.Object);
        
        // Act & Assert
        await Assert.ThrowsAsync<BookIdNotSetException>(() => handler.ExecuteAsync());
    }

    [Fact]
    public async Task ExecuteAsync_WithNonExistentBook_ShouldThrowBookNotFoundException()
    {
        // Arrange
        var mockRepository = new Mock<IBookRepository>();
        mockRepository.Setup(repo => repo.GetBookByIdAsync(It.IsAny<int>())).ReturnsAsync((Book)null!);
        var handler = new BuyCommandHandler(mockRepository.Object) { Id = 1 };
        
        // Act & Assert
        await Assert.ThrowsAsync<BookNotFoundException>(() => handler.ExecuteAsync());
    }

    [Fact]
    public async Task ExecuteAsync_WithZeroQuantity_ShouldThrowBookOverException()
    {
        // Arrange
        var mockRepository = new Mock<IBookRepository>();
        mockRepository.Setup(repo => repo.GetBookByIdAsync(It.IsAny<int>())).ReturnsAsync(new Book { Id = 1, Title = "Test Book", Quantity = 0 });
        var handler = new BuyCommandHandler(mockRepository.Object) { Id = 1 };
        
        // Act & Assert
        await Assert.ThrowsAsync<BookOverException>(() => handler.ExecuteAsync());
    }

    [Fact]
    public async Task ExecuteAsync_WithValidBook_ShouldDecrementQuantityAndSaveChanges()
    {
        // Arrange
        var mockRepository = new Mock<IBookRepository>();
        var book = new Book { Id = 1, Title = "Test Book", Quantity = 2 };
        mockRepository.Setup(repo => repo.GetBookByIdAsync(It.IsAny<int>())).ReturnsAsync(book);
        var handler = new BuyCommandHandler(mockRepository.Object) { Id = 1 };

        // Act
        var result = await handler.ExecuteAsync();

        // Assert
        mockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once());
        Assert.Equal($"Книга с id: 1 куплена (Test Book)", result);
    }
}