using Domain.Entities;
using Mapster;

namespace Application.Operations.Addresses;

public class AddressMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Address, AddressResponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.AdvertId, src => src.Advert.Id)
            .Map(dest => dest.City, src => src.City)
            .Map(dest => dest.Street, src => src.Street)
            .Map(dest => dest.Province, src => src.Province)
            .Map(dest => dest.House, src => src.House)
            .Map(dest => dest.GpsPosition, src => src.GpsPosition);
    }
}