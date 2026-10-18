using Microsoft.EntityFrameworkCore;

public class ExerciseTypeService : IExerciseTypeService
{
    private readonly AppDbContext _db;
    public ExerciseTypeService(AppDbContext db) => _db = db;

    public async Task<List<ExerciseType>> GetAllAsync() =>
        await _db.ExerciseTypes.AsNoTracking().OrderBy(t => t.Name).ToListAsync();
}
