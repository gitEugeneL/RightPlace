namespace Application.Operations.Addresses;

public record AddressResponse(
    Guid Id,
    Guid AdvertId,
    string City,
    string Street,
    string Province,
    string House,
    string? GpsPosition
);