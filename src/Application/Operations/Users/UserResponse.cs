namespace Application.Operations.Users;

public record UserResponse(
    Guid Id,
    string Email,
    string? FirstName,
    string? LastName,
    string? Phone,
    DateOnly? DateOfBirth
);