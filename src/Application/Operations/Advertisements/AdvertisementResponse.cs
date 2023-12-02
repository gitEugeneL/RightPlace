namespace Application.Operations.Advertisements;

public record AdvertisementResponse(
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
