public interface IHydrationLogService
{
    Task<HydrationLogEntry?> CreateAsync(Guid userId, HydrationLogCreateDto dto);
    Task<PagedResult<HydrationLogEntry>> GetAllForUserAsync(Guid userId, DateTime? from, DateTime? to, int page, int pageSize);
    Task<bool> DeleteAsync(Guid userId, Guid logId);
}