namespace BookStoreApp.Application.Exceptions;

public class BookOverException(int bookId) : BookStoreExceptionBase($"Книга с id: {bookId} закончилась");