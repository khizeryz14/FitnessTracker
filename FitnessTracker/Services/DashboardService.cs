using Microsoft.EntityFrameworkCore;

public class DashboardService : IDashboardService
{
    private readonly AppDbContext _db;
    private readonly TimeProvider _clock;

    public DashboardService(AppDbContext db, TimeProvider clock)
    {
        _db = db;
        _clock = clock;
    }

    public async Task<DashboardDto?> GetAsync(Guid userId, DateOnly? date)
    {
        var user = await _db.Users.FindAsync(userId);
        if (user is null) return null;

        var now = _clock.GetUtcNow().UtcDateTime;
        var day = date ?? DateOnly.FromDateTime(now);

        // UTC day window: [start, end)
        var start = DateTime.SpecifyKind(day.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc);
        var end = start.AddDays(1);

        var caloriesIn = await _db.FoodLogEntries
            .Where(f => f.UserId == userId && f.LoggedTimestamp >= start && f.LoggedTimestamp < end)
            .SumAsync(f => f.CaloriesConsumed);

        var exerciseBurned = await _db.ExerciseLogEntries
            .Where(e => e.UserId == userId && e.LoggedTimestamp >= start && e.LoggedTimestamp < end)
            .SumAsync(e => e.CaloriesBurned);

        var hydrationMl = await _db.HydrationLogEntries
            .Where(h => h.UserId == userId && h.LoggedTimestamp >= start && h.LoggedTimestamp < end)
            .SumAsync(h => h.AmountMl);

        var age = BmrCalculator.CalculateAge(user.DateOfBirth, DateOnly.FromDateTime(now));
        var bmrPerDay = BmrCalculator.CalculateBmr(user.Weight, user.Height, age, user.Sex);

        // Compute-on-read: past days clamp to 24h, future days to 0h, today is partial.
        var hoursElapsed = Math.Clamp((now - start).TotalHours, 0, 24);
        var bmrSoFar = bmrPerDay * hoursElapsed / 24.0;

        return new DashboardDto
        {
            Date = day,
            CaloriesIn = caloriesIn,
            ExerciseCaloriesBurned = exerciseBurned,
            BmrPerDay = bmrPerDay,
            BmrBurnedSoFar = bmrSoFar,
            NetCalories = caloriesIn - (exerciseBurned + bmrSoFar),
            DailyCalorieTarget = bmrPerDay * BmrCalculator.ActivityMultiplier(user.ActivityLevel),
            HydrationMl = hydrationMl,
            HydrationGoalMl = HydrationCalculator.DailyGoalMl(user.Weight)   // rule of thumb: ~35 ml per kg
        };
    }
}