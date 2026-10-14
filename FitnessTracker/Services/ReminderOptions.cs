public class ReminderOptions
{
    public int IntervalMinutes { get; set; } = 60;    // how often the worker wakes up
    public int MinGapMinutes { get; set; } = 180;     // no log for this long AND no reminder this recently
}