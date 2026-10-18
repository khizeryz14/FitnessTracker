using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ExerciseTypesController : ControllerBase
{
    private readonly IExerciseTypeService _service;
    public ExerciseTypesController(IExerciseTypeService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var types = await _service.GetAllAsync();
        return Ok(types.Select(t => new ExerciseTypeResponseDto
        {
            Id = t.Id, Name = t.Name, MetValue = t.MetValue
        }));
    }
}