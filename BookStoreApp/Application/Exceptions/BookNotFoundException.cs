namespace BookStoreApp.Application.Exceptions;

public class BookNotFoundException(int bookId) : BookStoreExceptionBase($"Книга с id: {bookId} не найдена");