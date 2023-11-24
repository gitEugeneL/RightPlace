using Application.Users.Commands;
using Domain.Entities;
using Mapster;

namespace Application.Users;

public class UserMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, UserResponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.FirstName, src => src.FirstName)
            .Map(dest => dest.LastName, src => src.LastName)
            .Map(dest => dest.Phone, src => src.Phone)
            .Map(dest => dest.DateOfBirth, src => src.DateOfBirth);
    }
}