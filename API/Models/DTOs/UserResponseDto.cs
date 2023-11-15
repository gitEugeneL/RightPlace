namespace API.Models.DTOs;

public record UserResponseDto
{
    public required Guid Id { get; set; }
    public required string Email { get; set; }
}