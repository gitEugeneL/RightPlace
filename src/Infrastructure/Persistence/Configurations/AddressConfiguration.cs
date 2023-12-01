using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.Property(address => address.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(address => address.Street)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(address => address.Province)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(address => address.House)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(address => address.GpsPosition)
            .HasMaxLength(250);

        builder.Property(address => address.Updated)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // one to one -------------------------------------------------------------------
        builder.HasOne<Advertisement>(address => address.Advertisement)
            .WithOne(advertisement => advertisement.Address)
            .OnDelete(DeleteBehavior.Cascade);
    }
}