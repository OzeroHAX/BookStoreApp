using BookStoreApp.Application.Base;
using BookStoreApp.Application.Exceptions;
using BookStoreApp.Application.Metadata;
using BookStoreApp.Application.Repositories;

namespace BookStoreApp.Application.Commands;

[Command(Name = "buy", Description = "Купить книгу")]
public class BuyCommandHandler(IBookRepository repository) : ICommandHandler
{
    [CommandOption(Name = "--id", Description = "Идентификатор книги")]
    public int? Id { get; set; }
    
    public async Task<string> ExecuteAsync()
    {
        if (Id == null) throw new BookIdNotSetException();
        
        var book = await repository.GetBookByIdAsync(Id.Value);
        
        if (book == null) throw new BookNotFoundException(Id.Value);
        if (book.Quantity <= 0) throw new BookOverException(Id.Value);
        
        book.Quantity--;
        await repository.SaveChangesAsync();
        return $"Книга с id: {Id} куплена ({book.Title})";
    }
}