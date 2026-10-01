using System.Security.Claims;
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

        var foodEntry = new FoodLogResponseDto
        {
            Id = entry.Id,
            UserId = entry.UserId,
            FoodItemId = entry.FoodItemId,
            QuantityGrams = entry.QuantityGrams,
            LoggedTimestamp = entry.LoggedTimestamp,
            CaloriesConsumed = entry.CaloriesConsumed,
        };
        return Created($"/api/foodlog/{entry.Id}", foodEntry);
    }

    [HttpGet]
    public async Task<IActionResult> GetEntries()
    {
        var entries = await _service.GetAllForUserAsync(GetUserId());

        var response = entries.Select(e => new FoodLogResponseDto
        {
            Id = e.Id,
            UserId = e.UserId,
            FoodItemId = e.FoodItemId,
            QuantityGrams = e.QuantityGrams,
            LoggedTimestamp = e.LoggedTimestamp,
            CaloriesConsumed = e.CaloriesConsumed
        }).ToList();

        return Ok(response);
    }

    [HttpDelete("{logId}")]
    public async Task<IActionResult> Delete(Guid logId)
    {
        var deleted = await _service.DeleteAsync(GetUserId(), logId);
        return deleted ? NoContent() : NotFound();
    }
}