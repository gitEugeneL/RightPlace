using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
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

        builder.Property(user => user.DateOfBirth);
        
        builder.Property(user => user.Created)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        
        // todo add updated field logic
        
        // many to one relation ------------------------------------------------------
        builder.HasMany<RefreshToken>(user => user.RefreshTokens)
            .WithOne(refreshToken => refreshToken.User)
            .HasForeignKey(refreshToken => refreshToken.UserId); 
    }
}