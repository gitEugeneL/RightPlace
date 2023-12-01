using Infrastructure.Persistence;
using Type = Domain.Entities.Type;

namespace Infrastructure.Seeders;

public static class TypeSeeder
{
    public static void Seed(ApplicationDbContext context)
    {
        if (context.Types.Any()) 
            return;
        var types = GetTypes();
        context.Types.AddRange(types);
        context.SaveChangesAsync();
    }
    
    private enum TypeName
    {
        Rent,
        Buy
    }

    private static IEnumerable<Type> GetTypes()
    {
        return new List<Type>
        {
            new() { Name = TypeName.Rent.ToString() },
            new() { Name = TypeName.Buy.ToString() }
        };
    }
}