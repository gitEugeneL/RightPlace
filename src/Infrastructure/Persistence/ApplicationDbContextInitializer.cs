using Infrastructure.Seeders;

namespace Infrastructure.Persistence;

public static class ApplicationDbContextInitializer
{
    public static void Init(ApplicationDbContext context)
    {
        context.Database.EnsureCreated();
        
        // seed data
        UserSeeder.Seed(context);
    }
}