using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Users.Commands.UpdateUser;

public record UpdateUserCommand : IRequest<UserResponse>
{
    public Guid CurrentUserId { get; private set; }

    [MinLength(3)]
    [MaxLength(100)]
    public string? FirstName { get; init; }
    
    [MinLength(3)]
    [MaxLength(100)]
    public string? LastName { get; init; }
    
    [MaxLength(12)]
    [RegularExpression(
        "^[+]?\\d+$",
        ErrorMessage = "Phone number should start with + (optional) and contain only digits."
    )]
    public string? Phone { get; init; }
    
    [DataType(DataType.Date)]
    public DateOnly? DateOfBirth { get; init; }

    public void SetCurrentUserId(string id)
    {
        CurrentUserId = Guid.Parse(id);
    }
}