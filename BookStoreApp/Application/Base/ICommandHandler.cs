namespace BookStoreApp.Application.Base;

public interface ICommandHandler
{
    public abstract Task<string> ExecuteAsync(); 
}