public interface INotificationPublisher
{
    Task PublishAsync(Notification notification);
}