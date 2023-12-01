using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Operations.Authentications.Commands.Login;

public record LoginCommand : IRequest<AuthenticationResponse>
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    public required string Password { get; set; }
}