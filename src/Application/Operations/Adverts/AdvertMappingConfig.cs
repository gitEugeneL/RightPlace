using Domain.Entities;
using Mapster;

namespace Application.Operations.Adverts;

public class AdvertMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Advert, AdvertsResponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.Price, src => src.Price)
            .Map(dest => dest.CategoryId, src => src.CategoryId)
            .Map(dest => dest.TypeId, src => src.TypeId)
            .Map(dest => dest.AddressId, src => src.AddressId)
            .Map(dest => dest.InformationId, src => src.InformationId)
            .Map(dest => dest.Created, src => src.Created)
            .Map(dest => dest.Updated, src => src.Updated);
    }
}
