using Microsoft.EntityFrameworkCore;
using TaskManager.Models;

namespace TaskManager.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<TaskItem> TaskItems => Set<TaskItem>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
        .HasIndex(u => u.Username)
        .IsUnique();

        modelBuilder.Entity<TaskItem>()
            .HasIndex(t => new { t.ProjectId, t.Status});
    }
}