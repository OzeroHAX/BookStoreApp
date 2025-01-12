using BookStoreApp.Application.Base;
using BookStoreApp.Application.Exceptions;
using BookStoreApp.Application.Metadata;
using BookStoreApp.Extensions;

namespace BookStoreApp;

public class CommandsService(IEnumerable<ICommandHandler> handlers)
{
    public async Task ExecuteCommandByName(string name, string[] args)
    {
        var handler = handlers
            .FirstOrDefault(x => (x.GetType()
                .GetCustomAttributes(typeof(CommandAttribute), false)[0] as CommandAttribute)?.Name == name);
        if (handler == null)
        {
            Console.WriteLine("Неизвестная команда");
            return;
        }

        try
        {
            var result = await FillProperties(handler, args).ExecuteAsync();
            Console.WriteLine(result);
        }
        catch (BookStoreExceptionBase e)
        {
            e.WriteConsoleWarning();
        }
        catch (Exception e)
        {
            e.WriteConsoleError();
        }
    }

    public string?[] GetAvailableCommands()
    {
        var commands = handlers
            .Select(x => (x.GetType()
                .GetCustomAttributes(typeof(CommandAttribute), false)[0] as CommandAttribute)?.Name)
            .ToArray();

        return commands;
    }
    
    private ICommandHandler FillProperties(ICommandHandler handler, string[] handlerArguments)
    {
        var handlerType = handler.GetType();
        var handlerProperties = handlerType.GetProperties();

        var argumentDictionary = handlerArguments
            .Select(arg => arg.Split('='))
            .Where(p => p.Length == 2)
            .ToDictionary(p => p[0].Trim(), p => RemoveQuotes(p[1].Trim()));

        foreach (var property in handlerProperties)
        {
            if (property
                    .GetCustomAttributes(typeof(CommandOptionAttribute), false)
                    .FirstOrDefault() is CommandOptionAttribute attribute &&
                argumentDictionary.TryGetValue(attribute.Name, out var value))
            {
                var propType = property.PropertyType;
                if (property.PropertyType.FullName != null && property.PropertyType.FullName.Contains("System.Nullable"))
                {
                    propType = property.PropertyType.GenericTypeArguments[0];
                }
                var convertedValue = Convert.ChangeType(value, propType);
                property.SetValue(handler, convertedValue);
            }
        }
        return handler;
    }
    
    private static string RemoveQuotes(string str)
    {
        if (str.StartsWith("\"") && str.EndsWith("\""))
        {
            return str.Substring(1, str.Length - 2);
        }

        return str;
    }
}