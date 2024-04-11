using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

internal class InformationConfiguration : IEntityTypeConfiguration<Information>
{
    public void Configure(EntityTypeBuilder<Information> builder)
    {
        builder.Property(information => information.RoomCount)
            .IsRequired();
        
        builder.Property(information => information.Area)
            .IsRequired();

        builder.Property(information => information.Elevator)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(information => information.Balcony)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(information => information.YearOfConstruction)
            .IsRequired()
            .HasMaxLength(4);

        builder.Property(information => information.Floor)
            .HasMaxLength(3);

        builder.Property(information => information.EnergyEfficiencyRating)
            .HasMaxLength(5);
        
        /*** one to one ***/
        builder.HasOne<Advert>(information => information.Advert)
            .WithOne(advert => advert.Information)
            .OnDelete(DeleteBehavior.Cascade);
    }
}