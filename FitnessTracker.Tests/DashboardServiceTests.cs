using Microsoft.Extensions.Time.Testing;
using Xunit;

public class DashboardServiceTests
{
    [Fact]
    public async Task GetAsync_MidDay_AccruesHalfOfBmr_AndComputesNet()
    {
        using var testDb = new TestDb();
        var db = testDb.Context;

        var user = new User
        {
            Id = Guid.NewGuid(), Name = "T", Email = "t@test.com", PasswordHash = "x",
            Height = 1.75, Weight = 70, Sex = Sex.Male,
            DateOfBirth = new DateOnly(1996, 10, 13), ActivityLevel = ActivityLevel.Average
        };
        var food = new FoodItem { Id = Guid.NewGuid(), Name = "Test", CaloriesPer100g = 100 };
        db.Users.Add(user);
        db.FoodItems.Add(food);
        db.FoodLogEntries.Add(new FoodLogEntry
        {
            Id = Guid.NewGuid(), UserId = user.Id, FoodItemId = food.Id,
            QuantityGrams = 500, CaloriesConsumed = 500,
            LoggedTimestamp = new DateTime(2026, 10, 13, 8, 0, 0, DateTimeKind.Utc)
        });
        db.ExerciseLogEntries.Add(new ExerciseLogEntry
        {
            Id = Guid.NewGuid(), UserId = user.Id,
            ExerciseTypeId = Guid.Parse("ef861371-49cb-4fda-9cf7-f66b293a91d1"),   // seeded "Running"
            DurationMinutes = 30, CaloriesBurned = 200,
            LoggedTimestamp = new DateTime(2026, 10, 13, 9, 0, 0, DateTimeKind.Utc)
        });
        await db.SaveChangesAsync();

        var clock = new FakeTimeProvider(new DateTimeOffset(2026, 10, 13, 12, 0, 0, TimeSpan.Zero));
        var result = await new DashboardService(db, clock).GetAsync(user.Id, null);

        Assert.NotNull(result);
        Assert.Equal(1648.75, result!.BmrPerDay, precision: 2);
        Assert.Equal(824.375, result.BmrBurnedSoFar, precision: 2);    // noon = half the day
        Assert.Equal(-524.375, result.NetCalories, precision: 2);      // 500 - (200 + 824.375)
    }
}