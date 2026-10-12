using Microsoft.EntityFrameworkCore;

public class HydrationLogService : IHydrationLogService
{
    private readonly AppDbContext _db;
    public HydrationLogService(AppDbContext db) => _db = db;

    public async Task<HydrationLogEntry?> CreateAsync(Guid userId, HydrationLogCreateDto dto)
    {
        if (dto.AmountMl <= 0) return null;   // proper validation comes in Phase 4.5

        var entry = new HydrationLogEntry
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            AmountMl = dto.AmountMl,
            LoggedTimestamp = dto.LoggedTimestamp
        };
        _db.HydrationLogEntries.Add(entry);
        await _db.SaveChangesAsync();
        return entry;
    }

    public async Task<PagedResult<HydrationLogEntry>> GetAllForUserAsync(
        Guid userId, DateTime? from, DateTime? to, int page, int pageSize)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 50);

        var query = _db.HydrationLogEntries.Where(h => h.UserId == userId);
        if (from.HasValue) query = query.Where(h => h.LoggedTimestamp >= from.Value);
        if (to.HasValue)   query = query.Where(h => h.LoggedTimestamp <= to.Value);

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderByDescending(h => h.LoggedTimestamp)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<HydrationLogEntry>
        {
            Items = items, Page = page, PageSize = pageSize, TotalCount = totalCount
        };
    }

    public async Task<bool> DeleteAsync(Guid userId, Guid logId)
    {
        var entry = await _db.HydrationLogEntries.FindAsync(logId);
        if (entry is null || entry.UserId != userId) return false;

        _db.HydrationLogEntries.Remove(entry);
        await _db.SaveChangesAsync();
        return true;
    }
}