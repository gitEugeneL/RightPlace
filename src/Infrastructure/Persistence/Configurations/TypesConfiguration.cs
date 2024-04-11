using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Type = Domain.Entities.Type;

namespace Infrastructure.Persistence.Configurations;

internal class TypesConfiguration : IEntityTypeConfiguration<Type>
{
    public void Configure(EntityTypeBuilder<Type> builder)
    {
        builder.HasIndex(type => type.Name)
            .IsUnique();

        builder.Property(type => type.Name)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion<string>();
        
        /*** many to one ***/
        builder.HasMany<Advert>(type => type.Adverts)
            .WithOne(advert => advert.Type)
            .HasForeignKey(advert => advert.TypeId);
    }
}