using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

[Authorize]
public class NotificationHub : Hub { }

public class SignalRNotificationPublisher : INotificationPublisher
{
    private readonly IHubContext<NotificationHub> _hub;
    public SignalRNotificationPublisher(IHubContext<NotificationHub> hub) => _hub = hub;

    public Task PublishAsync(Notification n) =>
        _hub.Clients.User(n.UserId.ToString())
            .SendAsync("notification", NotificationResponseDto.From(n));
}