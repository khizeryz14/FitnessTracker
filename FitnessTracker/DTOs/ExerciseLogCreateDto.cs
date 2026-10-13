using System.ComponentModel.DataAnnotations;
public class ExerciseLogCreateDto
{
    public Guid ExerciseTypeId { get; set; }
    [Range(1, 1440)] public double DurationMinutes { get; set; }
    public DateTime LoggedTimestamp { get; set; }
}