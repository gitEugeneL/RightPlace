using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

internal class AdvertConfiguration : IEntityTypeConfiguration<Advert>
{
    public void Configure(EntityTypeBuilder<Advert> builder)
    {
        builder.HasIndex(advert => advert.Title)
            .IsUnique();
        
        builder.Property(advert => advert.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(advert => advert.Description)
            .IsRequired();

        builder.Property(advert => advert.Price)
            .IsRequired()
            .HasColumnType("decimal(18, 2)");
        
        /*** many to one ***/
        builder.HasMany<Image>(advert => advert.Images)
            .WithOne(image => image.Advert)
            .HasForeignKey(image => image.AdvertId);
    }
}