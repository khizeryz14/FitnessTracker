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
            CaloriesBurned = caloriesBurned
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

    public async Task<List<ExerciseLogEntry>> GetAllForUserAsync(Guid userId)
    {
        var logEntries = await _db.ExerciseLogEntries.Where(e => e.UserId == userId).ToListAsync();
        return logEntries;
    }
}