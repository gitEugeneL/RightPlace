namespace Infrastructure.Persistence;

public class ApplicationDbContextInitializer
{
    public static void Init(ApplicationDbContext context)
    {
        // todo seed data
        
        context.Database.EnsureCreated();
    }
}