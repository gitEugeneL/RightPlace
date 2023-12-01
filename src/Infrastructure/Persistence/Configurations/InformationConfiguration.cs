using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class InformationConfiguration : IEntityTypeConfiguration<Information>
{
    public void Configure(EntityTypeBuilder<Information> builder)
    {
        builder.Property(advertisement => advertisement.RoomCount)
            .IsRequired();

        builder.Property(advertisement => advertisement.RoomCount)
            .IsRequired();

        builder.Property(advertisement => advertisement.Area)
            .IsRequired();

        builder.Property(advertisement => advertisement.Elevator)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(advertisement => advertisement.Balcony)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(advertisement => advertisement.YearOfConstruction)
            .IsRequired()
            .HasMaxLength(4);

        builder.Property(advertisement => advertisement.Floor)
            .HasMaxLength(3);

        builder.Property(advertisement => advertisement.EnergyEfficiencyRating)
            .HasMaxLength(5);
        
        // one to one -------------------------------------------------------------------
        builder.HasOne<Advertisement>(information => information.Advertisement)
            .WithOne(advertisement => advertisement.Information)
            .OnDelete(DeleteBehavior.Cascade);
    }
}