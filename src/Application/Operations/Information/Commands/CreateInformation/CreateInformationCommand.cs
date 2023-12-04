using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Operations.Information.Commands.CreateInformation;

public class CreateInformationCommand : IRequest<InformationResponse>
{
    public Guid CurrentUserId { get; private set; }

    [Required]
    public required Guid AdvertId { get; init; }
    
    [Required]
    [Range(1, 15)]
    public required uint RoomCount { get; init; }
    
    [Required]
    [Range(10, 5000)]
    public required uint Area { get; init; }
    
    [Required]
    [RegularExpression(@"^\d{4}$", ErrorMessage = "must be a valid 4-digit year.")]
    public required short YearOfConstruction { get; init; }
  
    [MinLength(1)]
    [MaxLength(10)]
    public string? EnergyEfficiencyRating { get; init; } 
    
    public bool? Elevator { get; init; }

    public bool? Balcony { get; init; }
    
    public uint? Floor { get; init; }
    
    public void SetCurrentUserId(string id) => CurrentUserId = Guid.Parse(id);
}
