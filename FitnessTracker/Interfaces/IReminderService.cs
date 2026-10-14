public interface IReminderService
{
    Task<int> RunAsync(CancellationToken ct = default);   // returns how many reminders were created
}