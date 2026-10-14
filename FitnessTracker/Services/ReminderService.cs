using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

public class ReminderService : IReminderService
{
    private const string ReminderType = "HydrationReminder";

    private readonly AppDbContext _db;
    private readonly TimeProvider _clock;
    private readonly ReminderOptions _options;

    public ReminderService(AppDbContext db, TimeProvider clock, IOptions<ReminderOptions> options)
    {
        _db = db;
        _clock = clock;
        _options = options.Value;
    }

    public async Task<int> RunAsync(CancellationToken ct = default)
    {
        var now = _clock.GetUtcNow().UtcDateTime;
        var todayStart = now.Date;
        var cutoff = now.AddMinutes(-_options.MinGapMinutes);

        // One query: per user, today's total and last log time
        var perUser = await _db.HydrationLogEntries
            .Where(h => h.LoggedTimestamp >= todayStart)
            .GroupBy(h => h.UserId)
            .Select(g => new
            {
                UserId = g.Key,
                Total = g.Sum(h => h.AmountMl),
                Last = g.Max(h => h.LoggedTimestamp)
            })
            .ToListAsync(ct);

        var stale = perUser.Where(x => x.Last < cutoff).ToList();
        if (stale.Count == 0) return 0;

        var ids = stale.Select(x => x.UserId).ToList();

        var users = await _db.Users.Where(u => ids.Contains(u.Id)).ToDictionaryAsync(u => u.Id, ct);

        var recentlyReminded = (await _db.Notifications
            .Where(n => n.Type == ReminderType && n.CreatedAt >= cutoff && ids.Contains(n.UserId))
            .Select(n => n.UserId)
            .Distinct()
            .ToListAsync(ct)).ToHashSet();

        var created = new List<Notification>();
        foreach (var s in stale)
        {
            if (recentlyReminded.Contains(s.UserId)) continue;
            if (!users.TryGetValue(s.UserId, out var user)) continue;

            var goal = HydrationCalculator.DailyGoalMl(user.Weight);
            if (s.Total >= goal) continue;

            created.Add(new Notification
            {
                Id = Guid.NewGuid(),
                UserId = s.UserId,
                Type = ReminderType,
                Message = $"You're at {s.Total:0} of {goal:0} ml today. Time for some water!",
                CreatedAt = now
            });
        }

        _db.Notifications.AddRange(created);
        await _db.SaveChangesAsync(ct);
        return created.Count;
    }
}