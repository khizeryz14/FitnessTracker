using System.ComponentModel.DataAnnotations;
public class FoodItemManualCreateDto
{
    [Required, StringLength(200, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;

    [Range(0, 900)]   // 900 kcal/100g is roughly pure fat
    public double CaloriesPer100g { get; set; }
}