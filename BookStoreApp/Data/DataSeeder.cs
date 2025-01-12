using System.Text.Json;

namespace BookStoreApp.Data;

public static class DataSeeder
{
    private const string BooksFilePath = "books.json";

    public static void Seed(BookStoreContext context)
    {
        if (context.Books.Any()) return;

        var books = LoadBooksFromJson();
        if (books != null)
        {
            context.Books.AddRange(books);
            context.SaveChanges();
        }
    }

    private static List<Book>? LoadBooksFromJson()
    {
        if (!File.Exists(BooksFilePath))
        {
            Console.WriteLine($"Файл {BooksFilePath} не найден.");
            return null;
        }

        var json = File.ReadAllText(BooksFilePath);
        return JsonSerializer.Deserialize<List<Book>>(json);
    }
}