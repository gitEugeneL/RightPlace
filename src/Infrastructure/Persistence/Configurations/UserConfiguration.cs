using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(user => user.Email)
            .IsUnique();
        
        builder.Property(user => user.Email)
            .IsRequired()
            .HasMaxLength(250);
        
        builder.Property(user => user.FirstName)
            .HasMaxLength(100);
        
        builder.Property(user => user.LastName)
            .HasMaxLength(100);
        
        builder.Property(user => user.Phone)
            .HasMaxLength(12);
        
        /*** many to one relation ***/
        builder.HasMany<RefreshToken>(user => user.RefreshTokens)
            .WithOne(refreshToken => refreshToken.User)
            .HasForeignKey(refreshToken => refreshToken.UserId); 
        
        /*** many to one relation ***/
        builder.HasMany<Advert>(user => user.Adverts)
            .WithOne(advert => advert.User)
            .HasForeignKey(advert => advert.UserId);
    }
}