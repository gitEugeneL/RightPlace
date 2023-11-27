using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Type = Domain.Entities.Type;

namespace Infrastructure.Persistence.Configurations;

public class TypesConfiguration : IEntityTypeConfiguration<Type>
{
    public void Configure(EntityTypeBuilder<Type> builder)
    {
        builder.HasIndex(type => type.Name)
            .IsUnique();

        builder.Property(type => type.Name)
            .IsRequired()
            .HasMaxLength(50);
        
        // many to one ----------------------------------------------
        builder.HasMany<Advertisement>(type => type.Advertisements)
            .WithOne(advertisement => advertisement.Type)
            .HasForeignKey(advertisement => advertisement.TypeId);
    }
}