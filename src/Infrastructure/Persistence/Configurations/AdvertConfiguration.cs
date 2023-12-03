using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

internal class AdvertConfiguration : IEntityTypeConfiguration<Advert>
{
    public void Configure(EntityTypeBuilder<Advert> builder)
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