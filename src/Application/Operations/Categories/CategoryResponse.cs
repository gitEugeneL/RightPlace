namespace Application.Operations.Categories;

public record CategoryResponse(
    Guid Id,
    string Name,
    string? Description
);
