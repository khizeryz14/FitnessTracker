using Microsoft.EntityFrameworkCore;
public class NotificationService : INotificationService
{
    private readonly AppDbContext _db;
    public NotificationService(AppDbContext db) => _db = db;

    public async Task<List<Notification>> GetLatestForUserAsync(Guid userId, int count)
    {
        count = Math.Clamp(count, 1, 50);
        return await _db.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    public async Task<bool> MarkReadAsync(Guid userId, Guid notificationId)
    {
        var n = await _db.Notifications.FindAsync(notificationId);
        if (n is null || n.UserId != userId) return false;

        n.IsRead = true;
        await _db.SaveChangesAsync();
        return true;
    }
}