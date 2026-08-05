using Microsoft.EntityFrameworkCore;
using Teacher.Service.Core.Entities;

namespace Teacher.Service.DataAccess.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Core.Entities.Teacher> Teachers { get; set; }
    public DbSet<Core.Entities.Subject> Subjects { get; set; }
    public DbSet<Core.Entities.TeacherSubject> TeacherSubjects { get; set; }
    public DbSet<Core.Entities.TeacherPhone> TeacherPhones { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Core.Entities.Teacher>(entity =>
        {
            entity.Property(t => t.FullName).IsRequired().HasMaxLength(200);
        });

        modelBuilder.Entity<Core.Entities.TeacherPhone>(entity =>
        {
            entity.Property(p => p.PhoneNumber).IsRequired().HasMaxLength(20);

            entity.HasOne(p => p.Teacher)
                  .WithMany(t => t.Phones)
                  .HasForeignKey(p => p.TeacherId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Core.Entities.Subject>(entity =>
        {
            entity.Property(s => s.Name).IsRequired().HasMaxLength(150);
        });

        modelBuilder.Entity<Core.Entities.TeacherSubject>(entity =>
        {
            entity.HasOne(ts => ts.Teacher)
                  .WithMany(t => t.TeacherSubjects)
                  .HasForeignKey(ts => ts.TeacherId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ts => ts.Subject)
                  .WithMany(s => s.TeacherSubjects)
                  .HasForeignKey(ts => ts.SubjectId)
                  .OnDelete(DeleteBehavior.Cascade);

            
            entity.HasIndex(ts => new { ts.TeacherId, ts.SubjectId }).IsUnique();
        });

        base.OnModelCreating(modelBuilder);
    }


}

