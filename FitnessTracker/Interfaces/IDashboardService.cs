public interface IDashboardService
{
    Task<DashboardDto?> GetAsync(Guid userId, DateOnly? date);
}
