using Infrastructure.Seeders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public static class ApplicationDbContextInitializer
{
    public static void Init(ApplicationDbContext context)
    {
        if (!context.Database.CanConnect()) 
            return;
        
        // update db for docker composition
        context.Database.Migrate();

        // seed data
        UserSeeder.Seed(context);

        // context.Database.EnsureCreated();
    }
}