using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<ExerciseType> ExerciseTypes { get; set; }
    public DbSet<ExerciseLogEntry> ExerciseLogEntries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<ExerciseType>().HasData(
        new ExerciseType { Id = Guid.Parse("ef861371-49cb-4fda-9cf7-f66b293a91d1"), Name = "Running", MetValue = 9.8 },
        new ExerciseType { Id = Guid.Parse("0565c351-6a66-460e-9e11-4092d380b33a"), Name = "Walking", MetValue = 3.5 },
        new ExerciseType { Id = Guid.Parse("eb6e6e2f-0663-404a-8c1d-fd2f89c448b5"), Name = "Cycling", MetValue = 7.5 },
        new ExerciseType { Id = Guid.Parse("4ce70da8-8645-4494-a989-76f6ee85f5d4"), Name = "Swimming", MetValue = 8.0 }
    );
}
}