using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MapsterMapper;
using MediatR;

namespace Application.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserResponse>
{
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;

    public CreateUserCommandHandler(
        IMapper mapper, IPasswordHasher passwordHasher, IUserRepository userRepository, IRoleRepository roleRepository)
    {
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    public async Task<UserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.UserExistByEmailAsync(request.Email, cancellationToken))
            throw new AlreadyExistException(nameof(User), request.Email);

        _passwordHasher.CreatePasswordHash(request.Password, out var hash, out var salt);

        var newUser = await _userRepository.CreateUserAsync(
            new User
            {
                Email = request.Email,
                PasswordHash = hash,
                PasswordSalt = salt,
                Role = await _roleRepository
                    .GetRoleByValueAsync(RoleName.RoleUser, cancellationToken) ?? new Role { Value = RoleName.RoleUser }
            },
            cancellationToken
        );
        
        return _mapper.Map<UserResponse>(newUser);
    }
}