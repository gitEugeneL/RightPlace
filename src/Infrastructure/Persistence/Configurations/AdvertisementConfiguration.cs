using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class AdvertisementConfiguration : IEntityTypeConfiguration<Advertisement>
{
    public void Configure(EntityTypeBuilder<Advertisement> builder)
    {
        builder.HasIndex(advertisement => advertisement.Title)
            .IsUnique();
        
        builder.Property(advertisement => advertisement.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(advertisement => advertisement.Description)
            .IsRequired();

        builder.Property(advertisement => advertisement.Price)
            .IsRequired()
            .HasColumnType("decimal(18, 2)");
        
        builder.Property(advertisement => advertisement.Created)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}