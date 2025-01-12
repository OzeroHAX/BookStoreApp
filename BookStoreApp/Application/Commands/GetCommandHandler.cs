using BookStoreApp.Application.Base;
using BookStoreApp.Application.Metadata;
using BookStoreApp.Application.Repositories;

namespace BookStoreApp.Application.Commands;

[Command(Name = "get", Description = "Получить список книг.")]
public class GetCommandHandler(IBookRepository repository) : ICommandHandler
{
    [CommandOption(Name = "--title", Description = "Фильтр по названию")]
    public string? Title { get; set; }

    [CommandOption(Name = "--author", Description = "Фильтр по автору")]
    public string? Author { get; set; }

    [CommandOption(Name = "--date", Description = "Фильтр по дате (yyyy-MM-dd)")]
    public DateTime? Date { get; set; }

    [CommandOption(Name = "--order-by", Description = "Сортировка по полю [title|author|date|count]")]
    public string? OrderBy { get; set; }

    public async Task<string> ExecuteAsync()
    {
        var books = await repository.GetBooksAsync(Title, Author, Date, OrderBy);
        var result = string.Join("\n",
            books.Select(book =>
                $"ID: {book.Id}, Название: {book.Title}, Автор: {book.Author}, Дата: {book.PublishDate:yyyy-MM-dd}, Количество: {book.Quantity}"));
        return result;
    }
}