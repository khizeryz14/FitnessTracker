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
}