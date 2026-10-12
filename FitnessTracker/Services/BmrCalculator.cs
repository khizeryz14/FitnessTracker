public static class BmrCalculator
{
    // Mifflin-St Jeor. Height is stored in meters, the formula wants cm.
    public static double CalculateBmr(double weightKg, double heightM, int ageYears, Sex sex)
    {
        var bmr = 10 * weightKg + 6.25 * (heightM * 100) - 5 * ageYears;
        return sex == Sex.Male ? bmr + 5 : bmr - 161;
    }

    public static double ActivityMultiplier(ActivityLevel level) => level switch
    {
        ActivityLevel.Sedentary => 1.2,
        ActivityLevel.Low       => 1.375,
        ActivityLevel.Average   => 1.55,
        ActivityLevel.High      => 1.725,
        ActivityLevel.Athletic  => 1.9,
        _                       => 1.2
    };

    // "today" is a parameter so this stays pure and testable.
    public static int CalculateAge(DateOnly dateOfBirth, DateOnly today)
    {
        var age = today.Year - dateOfBirth.Year;
        if (dateOfBirth > today.AddYears(-age)) age--;   // birthday hasn't happened yet this year
        return age;
    }
}