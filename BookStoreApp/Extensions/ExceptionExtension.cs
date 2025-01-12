namespace BookStoreApp.Extensions;

public static class ExceptionExtension
{
    public static void WriteConsoleWarning(this Exception exception)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(exception.Message);
        Console.ForegroundColor = ConsoleColor.White;
    }
    
    public static void WriteConsoleError(this Exception exception)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(exception);
        Console.ForegroundColor = ConsoleColor.White;
    }
}