using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _service;
    public NotificationsController(INotificationService service) => _service = service;

    private Guid GetUserId() =>
        Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    private static NotificationResponseDto ToDto(Notification n) => new()
    {
        Id = n.Id, Type = n.Type, Message = n.Message, CreatedAt = n.CreatedAt, IsRead = n.IsRead
    };

    [HttpGet]
    public async Task<IActionResult> GetLatest(int count = 20)
    {
        var items = await _service.GetLatestForUserAsync(GetUserId(), count);
        return Ok(items.Select(ToDto).ToList());
    }

    [HttpPost("{id}/read")]
    public async Task<IActionResult> MarkRead(Guid id)
    {
        return await _service.MarkReadAsync(GetUserId(), id) ? NoContent() : NotFound();
    }
}