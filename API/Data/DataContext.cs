using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public required DbSet<User> Users { get; set; }
    public required DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>(eb =>
        {
            eb.HasIndex(r => r.Value).IsUnique();
            eb.Property(r => r.Value).HasMaxLength(50);
            // many to one
            eb.HasMany<User>(r => r.Users)
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleId);
        });

        modelBuilder.Entity<User>(eb =>
        {
            eb.HasIndex(u => u.Email).IsUnique();
            eb.Property(u => u.Email).HasMaxLength(250);
            eb.Property(u => u.FirstName).HasMaxLength(100);
            eb.Property(u => u.LastName).HasMaxLength(100);
            eb.Property(u => u.Phone).HasMaxLength(12);
            eb.Property(u => u.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });
    }
}