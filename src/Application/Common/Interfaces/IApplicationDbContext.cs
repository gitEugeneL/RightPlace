using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; set; }
   
    public DbSet<Role> Roles { get; set; }
    
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}