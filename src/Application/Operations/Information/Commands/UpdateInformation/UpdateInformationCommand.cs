using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Operations.Information.Commands.UpdateInformation;

public record UpdateInformationCommand : IRequest<InformationResponse>
{
    public Guid CurrentUserId { get; private set; }
    
    [Required]
    public required Guid InformationId { get; init; }
    
    [Range(1, 15)]
    public uint? RoomCount { get; init; }
    
    [RegularExpression(@"^\d{4}$", ErrorMessage = "must be a valid 4-digit year.")]
    public short? YearOfConstruction { get; init; }
    
    [MinLength(1)]
    [MaxLength(10)]
    public string? EnergyEfficiencyRating { get; init; } 
    
    public bool? Elevator { get; init; }
    
    public bool? Balcony { get; init; }
    
    public uint? Floor { get; init; }
    
    public void SetCurrentUserId(string id) => CurrentUserId = Guid.Parse(id);
}
