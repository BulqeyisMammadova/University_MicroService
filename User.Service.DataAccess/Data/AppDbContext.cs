using Microsoft.EntityFrameworkCore;
using User.Service.Core.Entities.Entity;

namespace User.Service.DataAccess.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Core.Entities.Entity.User> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<Permission> Permissions { get; set; } = null!;
    public DbSet<RolePermission> RolePermissions { get; set; } = null!;
    public DbSet<UserRole> UserRoles { get; set; } = null!;
    public DbSet<VerificationToken> VerificationTokens { get; set; } = null!;   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Core.Entities.Entity.User>(entity =>
        {
            entity.Property(u => u.FullName).IsRequired().HasMaxLength(200);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(200);
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.HasIndex(u => u.Email).IsUnique();
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(r => r.Name).IsRequired().HasMaxLength(100);
            entity.HasIndex(r => r.Name).IsUnique();
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.Property(p => p.Name).IsRequired().HasMaxLength(150);
            entity.HasIndex(p => p.Name).IsUnique();
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.HasIndex(rp => new { rp.RoleId, rp.PermissionId }).IsUnique();

            entity.HasOne(rp => rp.Role).WithMany(r => r.RolePermissions)
                  .HasForeignKey(rp => rp.RoleId).OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(rp => rp.Permission).WithMany(p => p.RolePermissions)
                  .HasForeignKey(rp => rp.PermissionId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasIndex(ur => new { ur.UserId, ur.RoleId }).IsUnique();

            entity.HasOne(ur => ur.User).WithMany(u => u.UserRoles)
                  .HasForeignKey(ur => ur.UserId).OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ur => ur.Role).WithMany(r => r.UserRoles)
                  .HasForeignKey(ur => ur.RoleId).OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<VerificationToken>(entity =>
        {
            entity.Property(vt => vt.Token).IsRequired().HasMaxLength(500);
            entity.HasIndex(vt => vt.Token).IsUnique();

            entity.HasOne(vt => vt.User)
                  .WithMany()
                  .HasForeignKey(vt => vt.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        base.OnModelCreating(modelBuilder);
    }
}



