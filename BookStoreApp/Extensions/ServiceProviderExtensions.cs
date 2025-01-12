using BookStoreApp.Application.Base;
using BookStoreApp.Application.Commands;
using BookStoreApp.Application.Helpers;
using BookStoreApp.Application.Repositories;
using BookStoreApp.Data;
using BookStoreApp.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace BookStoreApp.Extensions;

public static class ServiceProviderExtensions
{
    public static IServiceProvider RegisterBookStoreServices(this IServiceCollection services)
    {
        services.AddBookStoreContextAsync()
            .AddSingleton<IBookRepository, BookRepository>()
            .AddTransient<IRandomHelper, RandomHelper>()
            .AddTransient<CommandsService>()
            .AddTransient<ICommandHandler, GetCommandHandler>()
            .AddTransient<ICommandHandler, BuyCommandHandler>()
            .AddTransient<ICommandHandler, RestockCommandHandler>();

        return services.BuildServiceProvider();
    }

    public static CommandsService GetCommandsService(this IServiceProvider serviceProvider) => 
        serviceProvider.GetService<CommandsService>()!;
    
   private static IServiceCollection AddBookStoreContextAsync(this IServiceCollection collection)
    {
        var context = new BookStoreContext();
        collection.AddSingleton(context);
        return collection;
    }
}