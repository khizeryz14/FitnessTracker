public class FoodItem
{
    public Guid Id { get; set; }
    public int? FdcId { get; set; }          // null if manually created
    public string Name { get; set; }
    public double CaloriesPer100g { get; set; }
}