namespace BookStoreApp.Application.Exceptions;

public class IncorrectRestockBookCountException()
    : BookStoreExceptionBase($"Некорректное количество книг для пополнения");