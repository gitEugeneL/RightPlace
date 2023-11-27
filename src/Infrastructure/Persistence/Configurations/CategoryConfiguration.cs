using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasIndex(category => category.Name)
            .IsUnique();

        builder.Property(category => category.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(category => category.Description)
            .HasMaxLength(250);
        
        // many to one ------------------------------------------------------------
        builder.HasMany<Advertisement>(category => category.Advertisements)
            .WithOne(advertisement => advertisement.Category)
            .HasForeignKey(advertisement => advertisement.CategoryId);
    }
}