using Mapster;
using Type = Domain.Entities.Type;

namespace Application.Operations.Types;

public class TypeMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Type, TypeResponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name);
    }
}