public interface IFoodLogService
{
    Task<FoodLogEntry?> CreateAsync(Guid userId, FoodLogCreateDto dto);
    Task<PagedResult<FoodLogEntry>> GetAllForUserAsync(Guid userId, DateTime? from, DateTime? to, int page, int pageSize);
    Task<bool> DeleteAsync(Guid userId, Guid logId);
}