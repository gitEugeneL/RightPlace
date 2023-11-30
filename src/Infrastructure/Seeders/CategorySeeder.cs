using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Seeders;

public static class CategorySeeder
{
    public static void Seed(ApplicationDbContext context)
    {
        if (!context.Database.CanConnect()) 
            return;
        if (!context.Database.IsRelational()) 
            return;
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
            new Category { Name = CategoryName.Room.ToString() },
            new Category { Name = CategoryName.Apartment.ToString() },
            new Category { Name = CategoryName.House.ToString() },
            new Category { Name = CategoryName.CommercialSpace.ToString() },
        };
    }
}