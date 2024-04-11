namespace Infrastructure.Persistence;

public static class ApplicationDbContextInitializer
{
    public static void Init(ApplicationDbContext context)
    {
        context.Database.EnsureCreated();
        
        /*** Seed data ***/
        DataGenerator.Seed(context);
    }
}