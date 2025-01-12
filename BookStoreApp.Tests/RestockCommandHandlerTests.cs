using BookStoreApp.Application.Commands;
using BookStoreApp.Application.Exceptions;
using BookStoreApp.Application.Helpers;
using BookStoreApp.Application.Repositories;
using BookStoreApp.Data;
using Moq;

namespace BookStoreApp.Tests;

public class RestockCommandHandlerTests
{
    private readonly Mock<IBookRepository> _repositoryMock;
    private readonly RestockCommandHandler _handler;
    private readonly Mock<IRandomHelper> _random;

    public RestockCommandHandlerTests()
    {
        _repositoryMock = new Mock<IBookRepository>();
        _random = new Mock<IRandomHelper>();
        _handler = new RestockCommandHandler(_repositoryMock.Object, _random.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WithValidIdAndCount_ShouldUpdateQuantityAndReturnSuccessMessage()
    {
        // Arrange
        var bookId = 1;
        var count = 5;
        var book = new Book { Id = bookId, Title = "Test Book", Author = "Author", Quantity = 0 };
        
        _handler.Id = bookId;
        _handler.Count = count;

        _repositoryMock.Setup(repo => repo.GetBookByIdAsync(bookId)).ReturnsAsync(book);
        _repositoryMock.Setup(repo => repo.SaveChangesAsync()).Returns(Task.CompletedTask);

        // Act
        var result = await _handler.ExecuteAsync();

        // Assert
        Assert.Equal(5, book.Quantity);
        Assert.Equal($"Книга с id: {bookId} ({book.Title}, {book.Author}) успешно пополнена на {count} штук. Текущее количество: {book.Quantity}.", result);
    }

    [Fact]
    public async Task ExecuteAsync_WithRandomIdAndCount_ShouldUpdateQuantityAndReturnSuccessMessage()
    {
        // Arrange
        var bookId = 1;
        var count = 5;
        var newRandomAppendCount = 6;
        var book = new Book { Id = bookId, Title = "Test Book", Author = "Author", Quantity = count };
        
        _repositoryMock.Setup(repo => repo.CountIdBooks()).ReturnsAsync(1);
        _repositoryMock.Setup(repo => repo.GetBookByIdAsync(bookId)).ReturnsAsync(book);
        _repositoryMock.Setup(repo => repo.SaveChangesAsync()).Returns(Task.CompletedTask);
        _random.Setup(random => random.GenerateRandomNumber(1, 10)).Returns(newRandomAppendCount);
        _random.Setup(random => random.GenerateRandomNumber(1, 1)).Returns(1);

        // Act
        var result = await _handler.ExecuteAsync();

        // Assert
        Assert.Equal(count + newRandomAppendCount, book.Quantity);
        Assert.Equal($"Книга с id: {bookId} ({book.Title}, {book.Author}) успешно пополнена на {newRandomAppendCount} штук. Текущее количество: {book.Quantity}.", result);
    }

    [Fact]
    public async Task ExecuteAsync_WithInvalidCount_ShouldThrowIncorrectRestockBookCountException()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.CountIdBooks()).ReturnsAsync(10);
        _handler.Count = 0;

        // Act & Assert
        await Assert.ThrowsAsync<IncorrectRestockBookCountException>(() => _handler.ExecuteAsync());
    }

    [Fact]
    public async Task ExecuteAsync_WithNonExistentBookId_ShouldThrowBookNotFoundException()
    {
        // Arrange
        var bookId = 1;
        
        _handler.Id = bookId;

        _repositoryMock.Setup(repo => repo.GetBookByIdAsync(bookId)).ReturnsAsync((Book)null!);
        _random.Setup(random => random.GenerateRandomNumber(1, 10)).Returns(1);

        // Act & Assert
        await Assert.ThrowsAsync<BookNotFoundException>(() => _handler.ExecuteAsync());
    }
}