using Microsoft.EntityFrameworkCore;
using University.Service.Core.Entities; 

namespace University.Service.DataAccess.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
   
    public DbSet<Core.Entities.University> Universities { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Core.Entities.University>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Name).IsRequired().HasMaxLength(200);
        });

        base.OnModelCreating(modelBuilder);
    }


}



