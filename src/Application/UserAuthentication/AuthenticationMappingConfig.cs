using Application.Common.Models;
using Mapster;

namespace Application.UserAuthentication;

public class AuthenticationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<JwtToken, AuthenticationResponse>()
            .Map(dest => dest.JwtToken, src => src);
        
        config.NewConfig<CookieToken, AuthenticationResponse>()
            .Map(dest => dest.CookieToken, src => src);
    }
}