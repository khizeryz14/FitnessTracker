public class Notification
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }
    public User User { get; set; } = null!;   // EF fills this in; null! silences the warning
}