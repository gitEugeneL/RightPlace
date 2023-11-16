using System.ComponentModel.DataAnnotations;

namespace API.Models.DTOs.Auth;

public record AuthRequestDto
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    public required string Password { get; set; }
}