public interface INotificationService
{
    Task<List<Notification>> GetLatestForUserAsync(Guid userId, int count);
    Task<bool> MarkReadAsync(Guid userId, Guid notificationId);
}
