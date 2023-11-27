using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Type = Domain.Entities.Type;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public required DbSet<User> Users { get; set; }
    public required DbSet<Role> Roles { get; set; }
    public required DbSet<RefreshToken> RefreshTokens { get; set; }
    public required DbSet<Address> Addresses { get; set; }
    public required DbSet<Advertisement> Advertisements { get; set; }
    public required DbSet<Category> Categories { get; set; }
    public required DbSet<Type> Types { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new UserConfiguration());
        builder.ApplyConfiguration(new RoleConfiguration());
        builder.ApplyConfiguration(new RefreshTokenConfiguration());
        builder.ApplyConfiguration(new AddressConfiguration());
        builder.ApplyConfiguration(new AdvertisementConfiguration());
        builder.ApplyConfiguration(new CategoryConfiguration());
        builder.ApplyConfiguration(new TypesConfiguration());
        
        base.OnModelCreating(builder);
    }
    
    public override Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken token = default)
    {
        foreach (var entity in ChangeTracker
                     .Entries()
                     .Where(x => x is { Entity: BaseAuditableEntity, State: EntityState.Modified })
                     .Select(x => x.Entity)
                     .Cast<BaseAuditableEntity>())
        {
            entity.Updated = DateTime.UtcNow;
        }

        return base.SaveChangesAsync(acceptAllChangesOnSuccess, token);
    }
}
    