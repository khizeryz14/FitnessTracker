public class FoodLogEntry
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid FoodItemId { get; set; }
    public double QuantityGrams { get; set; }
    public DateTime LoggedTimestamp { get; set; }
    public double CaloriesConsumed { get; set; }   // calculated: (CaloriesPer100g / 100) × QuantityGrams

    public User User { get; set; }
    public FoodItem FoodItem { get; set; }
}