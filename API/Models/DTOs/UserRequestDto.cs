using System.ComponentModel.DataAnnotations;

namespace API.Models.DTOs;

public record UserRequestDto
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
    
    [Required]
    [MinLength(8)]
    [MaxLength(20)]
    [RegularExpression(
        @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d!$%@#£€*?&]+$", 
        ErrorMessage = "The password must contain at least one letter, one special character, and one digit.")
    ]
    public required string Password { get; set; } 
}