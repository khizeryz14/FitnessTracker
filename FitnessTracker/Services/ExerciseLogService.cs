using Microsoft.EntityFrameworkCore;
public class ExerciseLogService : IExerciseLogService

{
    private readonly AppDbContext _db;

    public ExerciseLogService(AppDbContext db)
    {
        _db = db;
    }
    

    public async Task<ExerciseLogEntry?> CreateAsync(Guid userId, ExerciseLogCreateDto dto)
    {
        var exerciseType = await _db.ExerciseTypes.FindAsync(dto.ExerciseTypeId);
        if (exerciseType == null) return null;

        var user = await _db.Users.FindAsync(userId);
        if (user == null) return null;

        var caloriesBurned = CalorieCalculator.CalculateCaloriesBurned(
            exerciseType.MetValue, user.Weight, dto.DurationMinutes);

        var logEntry = new ExerciseLogEntry
        {
            Id = Guid.NewGuid(),
            ExerciseTypeId = dto.ExerciseTypeId,
            UserId = userId,
            DurationMinutes = dto.DurationMinutes,
            LoggedTimestamp = dto.LoggedTimestamp,
            CaloriesBurned = caloriesBurned,
            ExerciseType = exerciseType
        };

        _db.ExerciseLogEntries.Add(logEntry);
        await _db.SaveChangesAsync();
        return logEntry;
    }

    public async Task<bool> DeleteAsync(Guid userId, Guid logId)
    {
        var logEntry = await _db.ExerciseLogEntries.FindAsync(logId);
        if(logEntry is null)
        {
            return false;
        }
        if (logEntry.UserId != userId)
        {
            return false;
        }
        _db.ExerciseLogEntries.Remove(logEntry);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<PagedResult<ExerciseLogEntry>> GetAllForUserAsync(
        Guid userId, DateTime? from, DateTime? to, int page, int pageSize)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 50);

        var query = _db.ExerciseLogEntries.Where(e => e.UserId == userId);

        if (from.HasValue) query = query.Where(e => e.LoggedTimestamp >= from.Value);
        if (to.HasValue)   query = query.Where(e => e.LoggedTimestamp <= to.Value);

        var totalCount = await query.CountAsync();

        var items = await query
            .Include(e => e.ExerciseType)
            .OrderByDescending(e => e.LoggedTimestamp)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<ExerciseLogEntry>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}