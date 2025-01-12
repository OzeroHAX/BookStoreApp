using Microsoft.Extensions.DependencyInjection;

namespace BookStoreApp.Data;

public static class DataExtensions
{
    public static async Task<IServiceProvider> ApplyBookStoreMigration(this IServiceProvider provider)
    {
        var context = provider.GetService<BookStoreContext>(); 
        await context!.EnsureDatabaseUpdateAsync();
        DataSeeder.Seed(context);
        return provider;
    }
}