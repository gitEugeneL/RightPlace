using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Operations.Addresses.Commands.CreateAddress;

public record CreateAddressCommand : IRequest<AddressResponse>
{
    public Guid CurrentUserId { get; private set; }

    [Required]
    public required Guid AdvertId { get; init; }

    [Required]
    [MaxLength(50)]
    public required string City { get; init; }
    
    [Required]
    [MaxLength(50)]
    public required string Street { get; init; }
    
    [Required]
    [MaxLength(50)]
    public required string Province { get; init; }
    
    [Required]
    [MaxLength(10)]
    public required string House { get; init; }
    
    [RegularExpression(
        @"^([-+]?\d{1,2}[.]\d+),\s*([-+]?\d{1,3}[.]\d+)$", 
        ErrorMessage = "Invalid GPS coordinates")
    ]
    public string? GpsPosition { get; init; } 
    
    public void SetCurrentUserId(string id) => CurrentUserId = Guid.Parse(id);
}
