using BookStoreApp.Data;
using BookStoreApp.Extensions;
using Microsoft.Extensions.DependencyInjection;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.InputEncoding = System.Text.Encoding.UTF8;

var services = await new ServiceCollection()
    .RegisterBookStoreServices()
    .ApplyBookStoreMigration();

var commands = string.Join(", ", services.GetCommandsService().GetAvailableCommands());
Console.WriteLine("Добро пожаловать в магазин книг!");
Console.WriteLine($"Доступные команды: {commands}. Введите 'exit' для выхода.");

while (true)
{
    Console.Write("\nВведите команду: ");
    var input = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(input)) continue;
    if (input.Equals("exit", StringComparison.OrdinalIgnoreCase)) break;

    var parts = input.Split("--", StringSplitOptions.RemoveEmptyEntries);
    var commandName = parts[0].ToLower().Trim();
    var arguments = parts.Skip(1).Select(x => $"--{x}").ToArray();

    await services.GetCommandsService().ExecuteCommandByName(commandName, arguments);
}