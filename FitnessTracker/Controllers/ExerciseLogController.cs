using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ExerciseLogController : ControllerBase
{
    private readonly IExerciseLogService _service;
    public ExerciseLogController(IExerciseLogService service) => _service = service;

    private Guid GetUserId() =>
        Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    [HttpPost]
    public async Task<IActionResult> Create(ExerciseLogCreateDto dto)
    {
        var entry = await _service.CreateAsync(GetUserId(), dto);
        if (entry == null) return BadRequest("Invalid exercise type.");

            var response = new ExerciseLogResponseDto
            {
                Id = entry.Id,
                ExerciseTypeId = entry.ExerciseTypeId,
                DurationMinutes = entry.DurationMinutes,
                LoggedTimestamp = entry.LoggedTimestamp,
                CaloriesBurned = entry.CaloriesBurned
            };

        return Created($"/api/exerciselog/{entry.Id}", response);
    }

    [HttpDelete("{logId}")]
    public async Task<IActionResult> Delete(Guid logId)
    {
        var delete = await _service.DeleteAsync(GetUserId(), logId);
        if(delete){
            return NoContent();
        }
        else
        {
            return NotFound();
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetEntries()
    {
        var logEntries = await _service.GetAllForUserAsync(GetUserId());
        var response = logEntries.Select(e => new ExerciseLogResponseDto
        {
            Id = e.Id,
            ExerciseTypeId = e.ExerciseTypeId,
            DurationMinutes = e.DurationMinutes,
            LoggedTimestamp = e.LoggedTimestamp,
            CaloriesBurned = e.CaloriesBurned
        }).ToList();
        return Ok(response);
    }
}