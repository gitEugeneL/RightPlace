using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasIndex(category => category.Name)
            .IsUnique();

        builder.Property(category => category.Name)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion<string>();

        builder.Property(category => category.Description)
            .HasMaxLength(250);
        
        // many to one ------------------------------------------------------------
        builder.HasMany<Advert>(category => category.Adverts)
            .WithOne(advert => advert.Category)
            .HasForeignKey(advert => advert.CategoryId);
    }
}