namespace Application.Categories;

public sealed record CategoryResponse
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
}