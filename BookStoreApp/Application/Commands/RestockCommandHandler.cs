using BookStoreApp.Application.Base;
using BookStoreApp.Application.Exceptions;
using BookStoreApp.Application.Helpers;
using BookStoreApp.Application.Metadata;
using BookStoreApp.Application.Repositories;

namespace BookStoreApp.Application.Commands;

[Command(Name = "restock", Description = "Пополнить запас книг")]
public class RestockCommandHandler(IBookRepository repository, IRandomHelper random) : ICommandHandler
{
    [CommandOption(Name = "--id", Description = "Идентификатор книги")]
    // ReSharper disable once MemberCanBePrivate.Global
    public int? Id { get; set; }

    [CommandOption(Name = "--count", Description = "Количество книг для пополнения запасов")]
    // ReSharper disable once MemberCanBePrivate.Global
    public int? Count { get; set; }
    
    private const int CountMaxRestock = 10;
    
    public async Task<string> ExecuteAsync()
    {
        Id ??= random.GenerateRandomNumber(1, await repository.CountIdBooks());
        Count ??= random.GenerateRandomNumber(1, CountMaxRestock);

        if (Count <= 0) throw new IncorrectRestockBookCountException();
        
        var book = await repository.GetBookByIdAsync(Id.Value);
        if (book == null) throw new BookNotFoundException(Id.Value);
            
        book.Quantity += Count.Value;
        await repository.SaveChangesAsync();

        return $"Книга с id: {Id.Value} ({book.Title}, {book.Author}) успешно пополнена на {Count.Value} штук. Текущее количество: {book.Quantity}.";
    }
}