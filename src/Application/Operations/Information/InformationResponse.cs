namespace Application.Operations.Information;

public record InformationResponse(
    Guid Id,
    Guid AdvertId,
    uint RoomCount,
    uint Area,
    short YearOfConstruction,
    bool Elevator,
    bool Balcony,
    uint? Floor,
    string? EnergyEfficiencyRating
);
