public class DashboardDto
{
    public DateOnly Date { get; set; }
    public double CaloriesIn { get; set; }
    public double ExerciseCaloriesBurned { get; set; }
    public double BmrPerDay { get; set; }
    public double BmrBurnedSoFar { get; set; }
    public double NetCalories { get; set; }          // in - (exercise + BMR so far)
    public double DailyCalorieTarget { get; set; }   // BMR x activity multiplier
    public double HydrationMl { get; set; }
    public double HydrationGoalMl { get; set; }
}