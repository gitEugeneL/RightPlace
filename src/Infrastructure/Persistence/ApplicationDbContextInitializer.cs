using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public static class ApplicationDbContextInitializer
{
    public static void Init(ApplicationDbContext context)
    {
        // update db for docker composition
        context.Database.Migrate();
        
        // todo seed data
        
        context.Database.EnsureCreated();
    }
}