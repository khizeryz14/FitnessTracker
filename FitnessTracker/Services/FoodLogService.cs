using Microsoft.EntityFrameworkCore;

public interface IFoodLogService
{
    Task<FoodLogEntry?> CreateAsync(Guid userId, FoodLogCreateDto dto);
    Task<List<FoodLogEntry>> GetAllForUserAsync(Guid userId);
    Task<bool> DeleteAsync(Guid userId, Guid logId);
}

public class FoodLogService : IFoodLogService
{
    private readonly AppDbContext _db;

    public FoodLogService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<FoodLogEntry?> CreateAsync(Guid userId, FoodLogCreateDto dto)
    {
        var foodItem = await _db.FoodItems.FindAsync(dto.FoodItemId);
        if (foodItem is null) return null;

        var caloriesConsumed = (foodItem.CaloriesPer100g / 100.0) * dto.QuantityGrams;

        var logEntry = new FoodLogEntry
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            FoodItemId = dto.FoodItemId,
            QuantityGrams = dto.QuantityGrams,
            LoggedTimestamp = dto.LoggedTimestamp,
            CaloriesConsumed = caloriesConsumed
        };

        _db.FoodLogEntries.Add(logEntry);
        await _db.SaveChangesAsync();
        return logEntry;
    }

    public async Task<List<FoodLogEntry>> GetAllForUserAsync(Guid userId)
    {
        return await _db.FoodLogEntries.Where(f => f.UserId == userId).ToListAsync();
    }

    public async Task<bool> DeleteAsync(Guid userId, Guid logId)
    {
        var logEntry = await _db.FoodLogEntries.FindAsync(logId);
        if (logEntry is null) return false;
        if (logEntry.UserId != userId) return false;

        _db.FoodLogEntries.Remove(logEntry);
        await _db.SaveChangesAsync();
        return true;
    }
}