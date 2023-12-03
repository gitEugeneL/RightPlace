namespace Application.Operations.Adverts;

public record AdvertsResponse(
    Guid Id,
    string Title,
    string Description,
    decimal Price,
    Guid CategoryId,
    Guid TypeId,
    Guid? AddressId,
    Guid? InformationId,
    DateTime Created,
    DateTime? Updated
);
