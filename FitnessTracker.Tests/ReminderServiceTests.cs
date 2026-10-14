using Microsoft.Extensions.Options;
using Microsoft.Extensions.Time.Testing;
using Xunit;

public class ReminderServiceTests
{
    private static readonly DateTimeOffset Now = new(2026, 10, 13, 15, 0, 0, TimeSpan.Zero);

    private static ReminderService CreateService(AppDbContext db) =>
        new(db, new FakeTimeProvider(Now), Options.Create(new ReminderOptions { MinGapMinutes = 180 }));

    private static async Task SeedAsync(AppDbContext db, double amountMl, int hourUtc)
    {
        var user = new User
        {
            Id = Guid.NewGuid(), Name = "T", Email = "t@test.com", PasswordHash = "x",
            Height = 1.75, Weight = 70, Sex = Sex.Male,
            DateOfBirth = new DateOnly(1996, 10, 13), ActivityLevel = ActivityLevel.Average
        };
        db.Users.Add(user);
        db.HydrationLogEntries.Add(new HydrationLogEntry
        {
            Id = Guid.NewGuid(), UserId = user.Id, AmountMl = amountMl,
            LoggedTimestamp = new DateTime(2026, 10, 13, hourUtc, 0, 0, DateTimeKind.Utc)
        });
        await db.SaveChangesAsync();
    }

    [Theory]
    [InlineData(250.0, 8, 1)]     // behind goal, nothing logged for 7h -> remind
    [InlineData(250.0, 14, 0)]    // behind goal, but logged 1h ago -> no reminder
    [InlineData(3000.0, 8, 0)]    // goal (2450 ml for 70 kg) already met -> no reminder
    public async Task Run_CreatesReminder_OnlyWhenBehindAndStale(double amountMl, int hourUtc, int expected)
    {
        using var testDb = new TestDb();
        await SeedAsync(testDb.Context, amountMl, hourUtc);

        var created = await CreateService(testDb.Context).RunAsync();

        Assert.Equal(expected, created);
    }

    [Fact]
    public async Task Run_DoesNotDuplicate_WithinMinGap()
    {
        using var testDb = new TestDb();
        await SeedAsync(testDb.Context, 250.0, 8);
        var service = CreateService(testDb.Context);

        var first = await service.RunAsync();
        var second = await service.RunAsync();

        Assert.Equal(1, first);
        Assert.Equal(0, second);
    }
}