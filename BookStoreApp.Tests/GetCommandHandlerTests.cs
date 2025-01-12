using BookStoreApp.Application.Commands;
using BookStoreApp.Application.Repositories;
using BookStoreApp.Data;
using Moq;

namespace BookStoreApp.Tests;

public class GetCommandHandlerTests
{
    private readonly Mock<IBookRepository> _mockRepository;
    private readonly GetCommandHandler _handler;

    public GetCommandHandlerTests()
    {
        _mockRepository = new Mock<IBookRepository>();
        _handler = new GetCommandHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task ExecuteAsync_NoFilters_DefaultSorting()
    {
        // Arrange
        var books = new List<Book>
        {
            new Book
            {
                Id = 1, Title = "Book A", Author = "Author X", PublishDate = new DateTime(2023, 1, 1), Quantity = 5
            },
            new Book
            {
                Id = 2, Title = "Book B", Author = "Author Y", PublishDate = new DateTime(2022, 1, 1), Quantity = 3
            }
        };
        _mockRepository.Setup(repo => repo.GetBooksAsync(null, null, null, null)).ReturnsAsync(books);

        // Act
        var result = await _handler.ExecuteAsync();

        // Assert
        var expectedOutput = "ID: 1, Название: Book A, Автор: Author X, Дата: 2023-01-01, Количество: 5\n" +
            "ID: 2, Название: Book B, Автор: Author Y, Дата: 2022-01-01, Количество: 3";
        Assert.Equal(expectedOutput, result);
    }

    [Fact]
    public async Task ExecuteAsync_TitleFilter()
    {
        // Arrange
        var books = new List<Book>
        {
            new Book
            {
                Id = 1, Title = "Book A", Author = "Author X", PublishDate = new DateTime(2023, 1, 1), Quantity = 5
            }
        };
        _mockRepository.Setup(repo => repo.GetBooksAsync("Book A", null, null, null)).ReturnsAsync(books);

        // Act
        _handler.Title = "Book A";
        var result = await _handler.ExecuteAsync();

        // Assert
        var expectedOutput = "ID: 1, Название: Book A, Автор: Author X, Дата: 2023-01-01, Количество: 5";
        Assert.Equal(expectedOutput, result);
    }

    [Fact]
    public async Task ExecuteAsync_AuthorFilter()
    {
        // Arrange
        var books = new List<Book>
        {
            new Book
            {
                Id = 1, Title = "Book A", Author = "Author X", PublishDate = new DateTime(2023, 1, 1), Quantity = 5
            }
        };
        _mockRepository.Setup(repo => repo.GetBooksAsync(null, "Author X", null, null)).ReturnsAsync(books);

        // Act
        _handler.Author = "Author X";
        var result = await _handler.ExecuteAsync();

        // Assert
        var expectedOutput = "ID: 1, Название: Book A, Автор: Author X, Дата: 2023-01-01, Количество: 5";
        Assert.Equal(expectedOutput, result);
    }

    [Fact]
    public async Task ExecuteAsync_DateFilter()
    {
        // Arrange
        var books = new List<Book>
        {
            new Book
            {
                Id = 1, Title = "Book A", Author = "Author X", PublishDate = new DateTime(2023, 1, 1), Quantity = 5
            }
        };
        _mockRepository.Setup(repo => repo.GetBooksAsync(null, null, new DateTime(2023, 1, 1), null))
            .ReturnsAsync(books);

        // Act
        _handler.Date = new DateTime(2023, 1, 1);
        var result = await _handler.ExecuteAsync();

        // Assert
        var expectedOutput = "ID: 1, Название: Book A, Автор: Author X, Дата: 2023-01-01, Количество: 5";
        Assert.Equal(expectedOutput, result);
    }

    [Fact]
    public async Task ExecuteAsync_OrderByTitle()
    {
        // Arrange
        var books = new List<Book>
        {
            new Book
            {
                Id = 2, Title = "Book B", Author = "Author Y", PublishDate = new DateTime(2022, 1, 1), Quantity = 3
            },
            new Book
            {
                Id = 1, Title = "Book A", Author = "Author X", PublishDate = new DateTime(2023, 1, 1), Quantity = 5
            }
        };
        _mockRepository.Setup(repo => repo.GetBooksAsync(null, null, null, "title")).ReturnsAsync(books);

        // Act
        _handler.OrderBy = "title";
        var result = await _handler.ExecuteAsync();

        // Assert
        var expectedOutput = "ID: 2, Название: Book B, Автор: Author Y, Дата: 2022-01-01, Количество: 3\n" +
                             "ID: 1, Название: Book A, Автор: Author X, Дата: 2023-01-01, Количество: 5";
        Assert.Equal(expectedOutput, result);
    }

    [Fact]
    public async Task ExecuteAsync_OrderByAuthor()
    {
        // Arrange
        var books = new List<Book>
        {
            new Book
            {
                Id = 1, Title = "Book B", Author = "Author X", PublishDate = new DateTime(2022, 1, 1), Quantity = 3
            },
            new Book
            {
                Id = 2, Title = "Book A", Author = "Author Y", PublishDate = new DateTime(2023, 1, 1), Quantity = 5
            }
        };
        _mockRepository.Setup(repo => repo.GetBooksAsync(null, null, null, "author")).ReturnsAsync(books);

        // Act
        _handler.OrderBy = "author";
        var result = await _handler.ExecuteAsync();

        // Assert
        var expectedOutput = "ID: 1, Название: Book B, Автор: Author X, Дата: 2022-01-01, Количество: 3\n" +
                             "ID: 2, Название: Book A, Автор: Author Y, Дата: 2023-01-01, Количество: 5";
        Assert.Equal(expectedOutput, result);
    }

    [Fact]
    public async Task ExecuteAsync_OrderByDate()
    {
        // Arrange
        var books = new List<Book>
        {
            new Book
            {
                Id = 2, Title = "Book B", Author = "Author Y", PublishDate = new DateTime(2022, 1, 1), Quantity = 3
            },
            new Book
            {
                Id = 1, Title = "Book A", Author = "Author X", PublishDate = new DateTime(2023, 1, 1), Quantity = 5
            }
        };
        _mockRepository.Setup(repo => repo.GetBooksAsync(null, null, null, "date")).ReturnsAsync(books);

        // Act
        _handler.OrderBy = "date";
        var result = await _handler.ExecuteAsync();

        // Assert
        var expectedOutput = "ID: 2, Название: Book B, Автор: Author Y, Дата: 2022-01-01, Количество: 3\n" +
                             "ID: 1, Название: Book A, Автор: Author X, Дата: 2023-01-01, Количество: 5";
        Assert.Equal(expectedOutput, result);
    }

    [Fact]
    public async Task ExecuteAsync_OrderByQuantity()
    {
        // Arrange
        var books = new List<Book>
        {
            new Book
            {
                Id = 2, Title = "Book B", Author = "Author Y", PublishDate = new DateTime(2022, 1, 1), Quantity = 3
            },
            new Book
            {
                Id = 1, Title = "Book A", Author = "Author X", PublishDate = new DateTime(2023, 1, 1), Quantity = 5
            }
        };
        _mockRepository.Setup(repo => repo.GetBooksAsync(null, null, null, "quantity")).ReturnsAsync(books);

        // Act
        _handler.OrderBy = "quantity";
        var result = await _handler.ExecuteAsync();

        // Assert
        var expectedOutput = "ID: 2, Название: Book B, Автор: Author Y, Дата: 2022-01-01, Количество: 3\n" +
                             "ID: 1, Название: Book A, Автор: Author X, Дата: 2023-01-01, Количество: 5";
        Assert.Equal(expectedOutput, result);
    }
}