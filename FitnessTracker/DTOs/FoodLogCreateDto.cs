using System.ComponentModel.DataAnnotations;
public class FoodLogCreateDto
{
    public Guid FoodItemId { get; set; }
    [Range(1, 10000)] public double QuantityGrams { get; set; }
    public DateTime LoggedTimestamp { get; set; }
}