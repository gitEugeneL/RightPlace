namespace API.Models.DTOs.User;

public record UserResponseDto
{
    public required Guid Id { get; set; }
    public required string Email { get; set; }
}