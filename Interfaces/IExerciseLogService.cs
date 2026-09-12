public interface IExerciseLogService
{
    Task<ExerciseLogEntry?> CreateAsync(Guid userId, ExerciseLogCreateDto dto);
    Task<List<ExerciseLogEntry>> GetAllForUserAsync(Guid userId);
    Task<bool> DeleteAsync(Guid userId, Guid logId);
}