namespace BookStoreApp.Application.Metadata;

[AttributeUsage(AttributeTargets.Property)]
public class CommandOptionAttribute : Attribute
{
    public required string Name { get; set; }
    public required string Description { get; set; }
}