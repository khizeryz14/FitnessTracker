public class NotificationResponseDto
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }

    public static NotificationResponseDto From(Notification n) => new()
    {
        Id = n.Id, Type = n.Type, Message = n.Message, CreatedAt = n.CreatedAt, IsRead = n.IsRead
    };
}

