public interface IExerciseLogService
{
    Task<ExerciseLogEntry?> CreateAsync(Guid userId, ExerciseLogCreateDto dto);
    Task<PagedResult<ExerciseLogEntry>> GetAllForUserAsync(
        Guid userId, DateTime? from, DateTime? to, int page, int pageSize);
    Task<bool> DeleteAsync(Guid userId, Guid logId);
}