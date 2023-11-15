using System.Text;
using API.Entities;
using API.Exceptions;
using API.Models.DTOs;
using API.Repositories;

namespace API.Services;

public interface IUserService
{
    Task<UserResponseDto> Create(UserRequestDto dto);
}

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    public UserService(IUserRepository userRepository, IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }
    
    public async Task<UserResponseDto> Create(UserRequestDto dto)
    {
        if (await _userRepository.UserExistAsync(dto.Email))
            throw new AlreadyExistException($"User: {dto.Email} already exists"); // todo add custom validation
        
        var user = new User
        {
            Email = dto.Email,
            PasswordHash = Encoding.UTF8.GetBytes(dto.Password),  // todo create password hasher 
            PasswordSalt = Encoding.UTF8.GetBytes(dto.Password), // todo create password hasher
            Role = await _roleRepository.GetRoleByValueAsync("ROLE_USER") ?? new Role { Value = "ROLE_USER" }
        };
        await _userRepository.CreateUserAsync(user);
        
        return new UserResponseDto // todo create mapper
        {
            Id = user.Id,
            Email = user.Email
        };
    }
}