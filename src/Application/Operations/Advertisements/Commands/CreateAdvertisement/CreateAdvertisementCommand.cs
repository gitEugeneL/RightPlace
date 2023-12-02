using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Operations.Advertisements.Commands.CreateAdvertisement;

public record CreateAdvertisementCommand : IRequest<AdvertisementResponse>
{
    public Guid CurrentUserId { get; private set; }
    
    [Required]
    public required Guid CategoryId { get; init; }
    
    [Required]
    public required Guid TypeId { get; init; }

    [Required]
    [MinLength(10)]
    [MaxLength(100)]
    public required string Title { get; init; }
    
    [Required]
    public required string Description { get; init; }

    [Required]
    [Range(10, double.MaxValue)]
    public required decimal Price { get; init; }

    public void SetCurrentUserId(string id) => CurrentUserId = Guid.Parse(id);
}