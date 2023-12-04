using Mapster;
using Info = Domain.Entities.Information;

namespace Application.Operations.Information;

public class InformationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Info, InformationResponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.AdvertId, src => src.Advert.Id)
            .Map(dest => dest.RoomCount, src => src.RoomCount)
            .Map(dest => dest.Area, src => src.Area)
            .Map(dest => dest.YearOfConstruction, src => src.YearOfConstruction)
            .Map(dest => dest.Elevator, src => src.Elevator)
            .Map(dest => dest.Balcony, src => src.Balcony)
            .Map(dest => dest.Floor, src => src.Floor)
            .Map(dest => dest.EnergyEfficiencyRating, 
                src => src.EnergyEfficiencyRating);
    }
}
