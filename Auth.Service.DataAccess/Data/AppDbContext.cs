using Microsoft.EntityFrameworkCore;
using Auth.Service.Core.Entities;

namespace Auth.Service.DataAccess.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.Property(r => r.Token).IsRequired().HasMaxLength(500);
            entity.Property(r => r.Email).IsRequired().HasMaxLength(200);
            entity.Property(r => r.Role)
                  .HasConversion<string>()
                  .HasMaxLength(50);
            entity.HasIndex(r => r.Token).IsUnique();
        });

        base.OnModelCreating(modelBuilder);
    }
}