using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.Data;

public class BookStoreContext : DbContext
{
    public DbSet<Book> Books { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=bookstore.db");
    }
    
    public async Task EnsureDatabaseUpdateAsync()
    {
        await Database.MigrateAsync();
    }
}