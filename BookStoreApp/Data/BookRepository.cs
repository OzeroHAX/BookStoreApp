using BookStoreApp.Application.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.Data;

public class BookRepository(BookStoreContext context) : IBookRepository
{
    public async Task<ICollection<Book>> GetBooksAsync(string? title = null, string? author = null, DateTime? date = null,
        string? orderBy = null)
    {
        var query = context.Books.AsNoTracking().AsQueryable();

        if (title != null) query = query.Where(b => b.Title.Contains(title));
        if (author != null) query = query.Where(b => b.Author.Contains(author));
        if (date != null) query = query.Where(b => b.PublishDate == date);
        query = orderBy switch
        {
            "title" => query.OrderBy(b => b.Title),
            "author" => query.OrderBy(b => b.Author),
            "date" => query.OrderBy(b => b.PublishDate),
            "count" => query.OrderBy(b => b.Quantity),
            _ => query
        };
        return await query.ToListAsync();
    }

    public async Task<Book?> GetBookByIdAsync(int id)
    {
      return await context.Books.FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task<int> CountIdBooks()
    {
        return await context.Books.AsNoTracking().CountAsync();
    }
}