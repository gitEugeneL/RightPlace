using API.Entities;
using API.Exceptions;
using API.Models.DTOs.User;
using API.Repositories;
using API.Security;

namespace API.Services;

public interface IUserService
{
    Task<UserResponseDto> Create(UserRequestDto dto);
}

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPasswordHasher _passwordHasher;
    
    public UserService(IUserRepository userRepository, IRoleRepository roleRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _passwordHasher = passwordHasher;
    }
    
    public async Task<UserResponseDto> Create(UserRequestDto dto)
    {
        if (await _userRepository.UserExistAsync(dto.Email))
            throw new AlreadyExistException($"User: {dto.Email} already exists"); // todo add custom validation

        _passwordHasher.CreatePasswordHash(dto.Password, out var hash, out var salt);

        var newUser = await _userRepository.CreateUserAsync(
            new User
            {
                Email = dto.Email,
                PasswordHash = hash,
                PasswordSalt = salt,
                Role = await _roleRepository.GetRoleByValueAsync("ROLE_USER") ?? new Role { Value = "ROLE_USER" }
            }
        );
        
        return new UserResponseDto // todo create mapper
        {
            Id = newUser.Id,
            Email = newUser.Email
        };
    }
}