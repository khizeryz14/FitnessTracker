using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FoodLogController : ControllerBase
{
    private readonly IFoodLogService _service;
    public FoodLogController(IFoodLogService service) => _service = service;

    private Guid GetUserId() =>
        Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    [HttpPost]
    public async Task<IActionResult> Create(FoodLogCreateDto dto)
    {
        var entry = await _service.CreateAsync(GetUserId(), dto);
        if (entry == null) return BadRequest("Invalid food item.");
        return Created($"/api/foodlog/{entry.Id}", entry);
    }

    [HttpGet]
    public async Task<IActionResult> GetEntries()
    {
        var entries = await _service.GetAllForUserAsync(GetUserId());
        return Ok(entries);
    }

    [HttpDelete("{logId}")]
    public async Task<IActionResult> Delete(Guid logId)
    {
        var deleted = await _service.DeleteAsync(GetUserId(), logId);
        return deleted ? NoContent() : NotFound();
    }
}