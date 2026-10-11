using Microsoft.EntityFrameworkCore;

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

    public async Task<PagedResult<FoodLogEntry>> GetAllForUserAsync(Guid userId, DateTime? from, DateTime? to, int page, int pageSize){
        
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 50);
        var query = _db.FoodLogEntries.Where(f => f.UserId == userId);

        if (from.HasValue) query = query.Where(f => f.LoggedTimestamp >= from.Value);
        if (to.HasValue)   query = query.Where(f => f.LoggedTimestamp <= to.Value);

        query = query.OrderByDescending(f => f.LoggedTimestamp).Skip((page - 1) * pageSize).Take(pageSize);
        var totalCount = await query.CountAsync();

        var queryItems = await query.ToListAsync();

        var toReturn = new PagedResult<FoodLogEntry>{
            Items = queryItems,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };

        return toReturn;
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