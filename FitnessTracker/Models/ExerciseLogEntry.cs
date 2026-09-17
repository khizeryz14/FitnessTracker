public class ExerciseLogEntry
{
    public Guid Id {get; set;}
    public Guid ExerciseTypeId {get; set;}
    public Guid UserId {get; set;}
    public double DurationMinutes {get; set;}
    public DateTime LoggedTimestamp {get; set;}
    public User User {get; set;}
    public ExerciseType ExerciseType {get; set;}
    public double CaloriesBurned { get; set; }
}