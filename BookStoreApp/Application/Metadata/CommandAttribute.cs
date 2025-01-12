namespace BookStoreApp.Application.Metadata;

[AttributeUsage(AttributeTargets.Class)]
public class CommandAttribute : Attribute
{
    public required string Name { get; set; }
    public required string Description { get; set; }
}