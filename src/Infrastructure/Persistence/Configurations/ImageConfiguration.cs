using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.Property(image => image.BucketName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(image => image.FileName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(image => image.FileType)
            .IsRequired()
            .HasMaxLength(20);
    }
}