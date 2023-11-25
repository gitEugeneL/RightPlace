using Domain.Entities;
using Infrastructure.Persistence;
using Infrastructure.Security;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Seeders;

public static class UserSeeder
{
    public static void Seed(ApplicationDbContext context)
    {
        if (!context.Database.CanConnect()) 
            return;
        if (!context.Database.IsRelational()) 
            return;
        if (context.Roles.Any() && context.Users.Any()) 
            return;
        var users = GetUsers();
        context.Users.AddRange(users);
        context.SaveChangesAsync();
    }

    private static IEnumerable<User> GetUsers()
    {
        var passwordHasher = new PasswordHasher();
        passwordHasher.CreatePasswordHash("defaultPassword1@", out var hash, out var salt);
        
        var role = new Role { Value = "ROLE_USER" };

        return new List<User>
        {
            new User
            {
                Email = "default@user.com",
                PasswordHash = hash,
                PasswordSalt = salt,
                Role = role,
                FirstName = "defaultFirstName",
                LastName = "defaultLastName",
                Phone = "+48258741369",
                DateOfBirth = new DateTime(1859, 12, 31)
            },

            new User
            {
                Email = "default1@user.com",
                PasswordHash = hash,
                PasswordSalt = salt,
                Role = role,
                FirstName = "default1FirstName",
                LastName = "default1LastName",
                Phone = "+48789654123",
                DateOfBirth = new DateTime(2023, 12, 20)
            },
        };
    }
}