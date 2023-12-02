using System.ComponentModel.DataAnnotations;
using Application.Common.Models;
using MediatR;

namespace Application.Operations.Advertisements.Queries.GetAllAdvertisements;

public record GetAllAdvertisementsQueryWithPagination : IRequest<PaginatedList<AdvertisementResponse>>
{
    public Guid? CategoryId { get; init; }
    public Guid? TypeId { get; init; }
    public string? City { get; init; }
    public Guid? OwnerId { get; init; }
    public bool SortOrderAsc { get; init; } = true;
    
    [RegularExpression(
        "^(?i)(price|created)$", 
        ErrorMessage = "SortBy must be either 'price' or 'created'")
    ]
    public string? SortBy { get; init; }
    
    [Range(1, int.MaxValue)]
    public int PageNumber { get; init; } = 1;
    
    [Range(5, 100)]
    public int PageSize { get; init; } = 10;
}
