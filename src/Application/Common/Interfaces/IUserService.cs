using Application.Common.Models.DTOs.User;

namespace Application.Common.Interfaces;

public interface IUserService
{
    Task<UserResponseDto> Create(UserRequestDto dto);
}