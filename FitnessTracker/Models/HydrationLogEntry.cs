public class HydrationLogEntry
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public double AmountMl { get; set; }
    public DateTime LoggedTimestamp { get; set; }
    public User User { get; set; }
}