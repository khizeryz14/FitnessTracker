public static class CalorieCalculator
{
    public static double CalculateCaloriesBurned(double metValue, double weightKg, double durationMinutes)
    {
        double durationHours = durationMinutes / 60.0;
        return metValue * weightKg * durationHours;
    }
}