using BookStoreApp.Data;

namespace BookStoreApp.Application.Repositories;

public interface IBookRepository
{
    Task<ICollection<Book>> GetBooksAsync(
        string? title = null, 
        string? author = null, 
        DateTime? date = null, 
        string? orderBy = null);
    
    Task<Book?> GetBookByIdAsync(int id);

    Task SaveChangesAsync();

    Task<int> CountIdBooks();
}