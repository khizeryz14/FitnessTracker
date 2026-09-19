using Xunit;
public class CalorieCalculatorTests
{
    [Fact]
    public void CalculateCaloriesBurned_ReturnsCorrectValue_ForKnownInputs()
    {
        // Arrange: pick numbers you can hand-calculate
        double metValue = 5.0;
        double weightKg = 75.0;
        double durationMinutes = 30;
        double expected = 187.5; // calculate this by hand: MET × weight × (minutes/60)

        // Act: call the real method
        double actual = CalorieCalculator.CalculateCaloriesBurned(metValue, weightKg, durationMinutes);

        // Assert: check it matches
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CalculateCaloriesBurned_ReturnsCorrectValue_ForZeroDuration()
    {
        // Arrange: pick numbers you can hand-calculate
        double metValue = 5.0;
        double weightKg = 75.0;
        double durationMinutes = 0;
        double expected = 0; // calculate this by hand: MET × weight × (minutes/60)

        // Act: call the real method
        double actual = CalorieCalculator.CalculateCaloriesBurned(metValue, weightKg, durationMinutes);

        // Assert: check it matches
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CalculateCaloriesBurned_ReturnsCorrectValue_ForZeroMet()
    {
        // Arrange: pick numbers you can hand-calculate
        double metValue = 0;
        double weightKg = 75.0;
        double durationMinutes = 30;
        double expected = 0; // calculate this by hand: MET × weight × (minutes/60)

        // Act: call the real method
        double actual = CalorieCalculator.CalculateCaloriesBurned(metValue, weightKg, durationMinutes);

        // Assert: check it matches
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CalculateCaloriesBurned_ReturnsCorrectValue_ForFractionalDuration()
    {
        // Arrange: pick numbers you can hand-calculate
        double metValue = 7.5;
        double weightKg = 75.0;
        double durationMinutes = 43;
        double expected = 403.125; // calculate this by hand: MET × weight × (minutes/60)

        // Act: call the real method
        double actual = CalorieCalculator.CalculateCaloriesBurned(metValue, weightKg, durationMinutes);

        // Assert: check it matches
        Assert.Equal(expected, actual);
    }
}