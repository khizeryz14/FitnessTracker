using Microsoft.EntityFrameworkCore;

public class FoodItemService : IFoodItemService
{
    private readonly AppDbContext _db;
    private readonly UsdaFoodClient _usdaClient;

    public FoodItemService(AppDbContext db, UsdaFoodClient usdaClient)
    {
        _db = db;
        _usdaClient = usdaClient;
    }

    public async Task<List<UsdaFood>> SearchAsync(string query)
    {
        return await _usdaClient.SearchFoodAsync(query);
    }

    public async Task<FoodItem> GetOrCreateFromUsdaAsync(int fdcId)
    {
        var existing = await _db.FoodItems.FirstOrDefaultAsync(f => f.FdcId == fdcId);
        if (existing is not null) return existing;

        var usdaFood = await _usdaClient.GetFoodDetailsAsync(fdcId);
        var energy = usdaFood?.FoodNutrients?.FirstOrDefault(n => n.Nutrient?.Name == "Energy")?.Amount ?? 0;

        var foodItem = new FoodItem
        {
            Id = Guid.NewGuid(),
            FdcId = fdcId,
            Name = usdaFood?.Description ?? "Unknown",
            CaloriesPer100g = energy
        };

        _db.FoodItems.Add(foodItem);
        await _db.SaveChangesAsync();
        return foodItem;
    }

    public async Task<FoodItem> CreateManualAsync(string name, double caloriesPer100g)
    {
        var foodItem = new FoodItem
        {
            Id = Guid.NewGuid(),
            FdcId = null,
            Name = name,
            CaloriesPer100g = caloriesPer100g
        };

        _db.FoodItems.Add(foodItem);
        await _db.SaveChangesAsync();
        return foodItem;
    }
}