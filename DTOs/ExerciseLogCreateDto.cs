public class ExerciseLogCreateDto
{
    public Guid ExerciseTypeId { get; set; }
    public double DurationMinutes { get; set; }
    public DateTime LoggedTimestamp { get; set; }
}