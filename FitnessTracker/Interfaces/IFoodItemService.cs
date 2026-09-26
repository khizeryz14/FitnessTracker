public interface IFoodItemService
{
    Task<List<UsdaFood>> SearchAsync(string query);
    Task<FoodItem> GetOrCreateFromUsdaAsync(int fdcId, string name, double caloriesPer100g);
    Task<FoodItem> CreateManualAsync(string name, double caloriesPer100g);
}