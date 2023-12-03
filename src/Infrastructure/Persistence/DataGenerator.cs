using Bogus;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Security;
using Type = Domain.Entities.Type;

namespace Infrastructure.Persistence;

public static class DataGenerator
{
    public static void Seed(ApplicationDbContext context)
    {
        if (context.Types.Any() || context.Categories.Any() || context.Users.Any() 
            || context.Roles.Any() || context.Adverts.Any())
            return;
        
        var passwordHasher = new PasswordHasher();
        passwordHasher.CreatePasswordHash("defaultPassword1@", out var hash, out var salt);
        
        var role = new Role { Value = RoleName.RoleUser };
        
        var categories = new List<Category>
        {
            new() { Name = "Room" },
            new() { Name = "Apartment" },
            new() { Name = "House" },
            new() { Name = "Commercial space" },
        };

        var types = new List<Type>
        {
            new() { Name = "Rent" },
            new() { Name = "Buy" }
        };
        
        var userGenerator = new Faker<User>()
            .RuleFor(user => user.Email, fake => fake.Person.Email)
            .RuleFor(user => user.FirstName, fake => fake.Person.FirstName)
            .RuleFor(user => user.LastName, fake => fake.Person.LastName)
            .RuleFor(user => user.Phone, fake => fake.Person.Phone)
            .RuleFor(user => user.Role, role)
            .RuleFor(user => user.PasswordHash, hash)
            .RuleFor(user => user.PasswordSalt, salt)
            .RuleFor(user => user.DateOfBirth, fake =>
                new DateOnly(fake.Person.DateOfBirth.Year, fake.Person.DateOfBirth.Month, fake.Person.DateOfBirth.Day));
        
        var advertGenerator = new Faker<Advert>()
            .RuleFor(advert => advert.Title, faker => $"{ faker.Lorem.Word() } real-estate")
            .RuleFor(advert => advert.Description, faker => faker.Lorem.Sentence())
            .RuleFor(advert => advert.Price, faker => Math.Round(faker.Random.Decimal(10000, 500000), 2))
            .RuleFor(advert => advert.Category, faker => faker.Random.ListItem(categories))
            .RuleFor(advert => advert.Type, faker => faker.Random.ListItem(types))
            .RuleFor(advert => advert.User, faker => userGenerator.Generate());
            // address
            // information
            
        var advert = advertGenerator.Generate(10);
        context.AddRange(advert);
        context.SaveChanges();
    }
}