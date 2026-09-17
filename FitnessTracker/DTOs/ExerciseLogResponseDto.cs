public class ExerciseLogResponseDto
{
    public Guid Id { get; set; }
    public Guid ExerciseTypeId { get; set; }
    public double DurationMinutes { get; set; }
    public DateTime LoggedTimestamp { get; set; }
    public double CaloriesBurned { get; set; }
}