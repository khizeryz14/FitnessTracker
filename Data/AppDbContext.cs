using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<ExerciseType> ExerciseTypes { get; set; }
    public DbSet<ExerciseLogEntry> ExerciseLogEntries { get; set; }
}