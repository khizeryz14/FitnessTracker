public class User
{
    public Guid Id {get; set;}
    public string Name {get; set;}
    public string Email {get; set;}
    public string PasswordHash { get; set; }
    public double Height {get; set;} // metric units (m)
    public double Weight {get; set;} // metric units (kg)
    public Sex Sex {get; set;} // (M/F)
    public DateOnly DateOfBirth {get; set;} // to calculate age

    public ActivityLevel ActivityLevel {get; set;}
    public ICollection<ExerciseLogEntry> ExerciseLogEntries { get; set; } = new List<ExerciseLogEntry>();
}

public enum ActivityLevel
    {
        Sedentary = 1,
        Low,
        Average,
        High,
        Athletic
    }

public enum Sex
{
    Male,
    Female
}