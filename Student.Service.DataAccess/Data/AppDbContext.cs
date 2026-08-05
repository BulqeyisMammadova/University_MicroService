using Microsoft.EntityFrameworkCore;
using Student.Service.Core.Entities;

namespace Student.Service.DataAccess.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Core.Entities.Student> Students { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Core.Entities.Student>(entity =>
        {
            entity.Property(s => s.FullName).IsRequired().HasMaxLength(200);
        });

        base.OnModelCreating(modelBuilder);
    }
}