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
    public async Task<IActionResult> GetEntries(DateTime? from, DateTime? to, int page = 1, int pageSize = 10)
    {
        var entries = await _service.GetAllForUserAsync(GetUserId(), from, to, page, pageSize);

        if (from.HasValue) from = DateTime.SpecifyKind(from.Value, DateTimeKind.Utc);
        if (to.HasValue)   to   = DateTime.SpecifyKind(to.Value, DateTimeKind.Utc);

        var response = new PagedResult<FoodLogResponseDto>
        {
            Items = entries.Items.Select(e => new FoodLogResponseDto
            {
                Id = e.Id,
                UserId = e.UserId,
                FoodItemId = e.FoodItemId,
                QuantityGrams = e.QuantityGrams,
                LoggedTimestamp = e.LoggedTimestamp,
                CaloriesConsumed = e.CaloriesConsumed
            }).ToList(),
            Page = entries.Page,
            PageSize = entries.PageSize,
            TotalCount = entries.TotalCount
        };


        return Ok(response);
    }

    [HttpDelete("{logId}")]
    public async Task<IActionResult> Delete(Guid logId)
    {
        var deleted = await _service.DeleteAsync(GetUserId(), logId);
        return deleted ? NoContent() : NotFound();
    }
}