using Microsoft.Extensions.Options;

public class HydrationReminderWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ReminderOptions _options;
    private readonly ILogger<HydrationReminderWorker> _logger;

    public HydrationReminderWorker(
        IServiceScopeFactory scopeFactory,
        IOptions<ReminderOptions> options,
        ILogger<HydrationReminderWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _options = options.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromMinutes(_options.IntervalMinutes));
        do
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();   // fresh scope = fresh DbContext per run
                var reminders = scope.ServiceProvider.GetRequiredService<IReminderService>();
                var created = await reminders.RunAsync(stoppingToken);
                _logger.LogInformation("Hydration reminder run done: {Count} created", created);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                // a failed run must not kill the loop
                _logger.LogError(ex, "Hydration reminder run failed");
            }
        }
        while (await timer.WaitForNextTickAsync(stoppingToken));
    }
}