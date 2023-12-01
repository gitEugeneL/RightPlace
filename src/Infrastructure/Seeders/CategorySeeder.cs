using Domain.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Seeders;

public static class CategorySeeder
{
    public static void Seed(ApplicationDbContext context)
    {
        if (context.Categories.Any()) 
            return;
        var categories = GetCategories();
        context.Categories.AddRange(categories);
        context.SaveChangesAsync();
    }

    private enum CategoryName
    {
        Room,
        Apartment,
        House,
        CommercialSpace
    }
    
    private static IEnumerable<Category> GetCategories()
    {
        return new List<Category>
        {
            new() { Name = CategoryName.Room.ToString() },
            new() { Name = CategoryName.Apartment.ToString() },
            new() { Name = CategoryName.House.ToString() },
            new() { Name = CategoryName.CommercialSpace.ToString() },
        };
    }
}