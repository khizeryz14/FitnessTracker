using Xunit;

public class BmrCalculatorTests
{
    [Fact]
    public void CalculateBmr_Male_MatchesHandCalculation()
    {
        // 10*70 + 6.25*175 - 5*30 + 5 = 1648.75
        var actual = BmrCalculator.CalculateBmr(70, 1.75, 30, Sex.Male);
        Assert.Equal(1648.75, actual, precision: 2);
    }

    [Fact]
    public void CalculateBmr_Female_MatchesHandCalculation()
    {
        // 10*70 + 6.25*175 - 5*30 - 161 = 1482.75
        var actual = BmrCalculator.CalculateBmr(70, 1.75, 30, Sex.Female);
        Assert.Equal(1482.75, actual, precision: 2);
    }

    [Theory]
    [InlineData("1996-10-14", "2026-10-13", 29)]   // birthday is tomorrow
    [InlineData("1996-10-13", "2026-10-13", 30)]   // birthday is today
    [InlineData("2000-02-29", "2026-02-28", 25)]   // leap-day birthday
    public void CalculateAge_HandlesBirthdayBoundaries(string dob, string today, int expected)
    {
        var actual = BmrCalculator.CalculateAge(DateOnly.Parse(dob), DateOnly.Parse(today));
        Assert.Equal(expected, actual);
    }
}