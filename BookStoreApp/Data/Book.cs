using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.Data;

public class Book
{
    [Key]
    public int Id { get; set; }
    public string Author { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public DateTime PublishDate { get; set; }
    public int Quantity { get; set; }
}